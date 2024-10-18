using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project
{
    public class Mantling : MonoBehaviour
    {

        Camera cam;
        PlayerMovement playerMovement;
        Climbing climb;
        PlayerActionManager actionManager;
        AbilityEventHandler animEventHandler;

        Vector3 enterVel; 
        Rigidbody rb;
        Vector3 LedgePos;
        Vector3 StartPos;
        Vector3 climbPos;
        public float MaxLedgeHeight;
        public float MantleSpeed;
        public float DistanceFromTransformToFeet;
        public CapsuleCollider col;
        public float vaultSpeed;
        public float vaultHeight;
        bool vault;
        public LayerMask Mantleable;
        public bool mantling;
        public GameObject lastWall;
        private float _mantleSpeed;
        public float slowClimbForce;
        public float forwardLerpFactor;
        public bool active;
        private void Start()
        {
            col = GetComponent<CapsuleCollider>();
            climb = GetComponent<Climbing>();
            playerMovement = GetComponent<PlayerMovement>();
            cam = GetComponent<HandleCam>().PC.GetComponent<Camera>();
            actionManager = GetComponent<PlayerActionManager>();
            animEventHandler = cam.GetComponentInChildren<AbilityEventHandler>();
            rb = GetComponent<Rigidbody>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            if(!active) return;
            
            if (collision.collider.gameObject == lastWall || collision.collider.gameObject.tag == "Projectile") return;
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 horForward = cam.transform.forward;
            horForward.y = 0; 
            horForward.Normalize();
            if(Vector3.Angle(horForward, -normal)<= 45)
            {
                bool ledgeAvailable = true;
                RaycastHit hit;
                if(Physics.Raycast(cam.transform.position + MaxLedgeHeight*Vector3.up, -normal, out hit, 1, Mantleable))
                {
                    ledgeAvailable = false;
                }
                //if (collision.gameObject.layer != LayerMask.GetMask("Ground")) return;

                //Need to check if space is pressed but gives weird results will revisit this soon 
                if (ledgeAvailable && (Input.GetKey(KeyCode.W)))
                {
                    Vector3 currentPos = cam.transform.position + Vector3.up * MaxLedgeHeight + Vector3.down * 0.05f;
                    while (!Physics.Raycast(currentPos, -normal, out hit, 1, Mantleable))
                    {
                        currentPos += Vector3.down * 0.05f;
                        if(currentPos.y < cam.transform.position.y-2f) {
                            break;
                        }
                    }
                    actionManager.OnAbility("Mantle");
                    animEventHandler.SetIk = false;
                    if (playerMovement.grounded) vault = true; else vault = false;
                    lastWall = collision.collider.gameObject;
                    mantling = true;
                    enterVel = -collision.relativeVelocity;
                    climb.enabled = false;
                    climbTime = 0;
                    rb.isKinematic = true;
                    playerMovement.enabled = false;
                    LedgePos = currentPos;
                    climbPos = CalculateClimbHeight(LedgePos) + new Vector3(enterVel.x * forwardLerpFactor, 0, enterVel.z * forwardLerpFactor);
                    StartPos = transform.position;
                    if (climbPos.y - StartPos.y - DistanceFromTransformToFeet <= vaultHeight || playerMovement.grounded)
                    {
                        vault = true;
                        _mantleSpeed = vaultSpeed;
                    }
                    else
                    {
                        vault = false;
                        _mantleSpeed = MantleSpeed;
                    }

                }

                }
            
            

        }

        public Vector3 CalculateClimbHeight(Vector3 LedgePos)
        {
            float yDistNeeded = LedgePos.y - transform.position.y + 1.2f * DistanceFromTransformToFeet;
            
            
            Vector3 climbPos = LedgePos + yDistNeeded * Vector3.up;
            return climbPos;
        }

        private void Update()
        {
            

            if (mantling && LedgePos != Vector3.zero)
            {
                DoMantle(LedgePos);

            }
            else
            {
                if (playerMovement.grounded) lastWall = null;
            }
        }

        float climbTime;

        public void DoMantle(Vector3 ledgePos)
        {
            
            animEventHandler.IKTarget.transform.DOLocalMove(animEventHandler.IKTarget.InverseTransformPoint(ledgePos + 0.3f * playerMovement.orientation.right), 0.1f);
            rb.MovePosition(Vector3.Lerp(StartPos, climbPos, climbTime));
            climbTime = Mathf.Clamp01(climbTime + Time.deltaTime * _mantleSpeed);
            if (transform.position.y >= climbPos.y)
            {
                
                DOTween.Clear(animEventHandler.IKTarget.transform);
                animEventHandler.SetIk = true;
                animEventHandler.SetIKToOrigin();
                mantling = false;
                playerMovement.enabled = true;
                rb.isKinematic = false;
                climb.enabled = false;
                
                if (Input.GetKey(KeyCode.Space) || vault)
                {
                    rb.AddForce(1000 * Vector3.up);
                    rb.velocity = enterVel;

                }
                else rb.AddForce(slowClimbForce * playerMovement.orientation.forward);

                climbTime = 0;
                LedgePos = Vector3.zero;
            } 
        }

    }
}
