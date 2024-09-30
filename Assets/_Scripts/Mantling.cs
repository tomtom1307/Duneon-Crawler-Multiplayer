using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Mantling : MonoBehaviour
    {

        Camera cam;
        PlayerMovement playerMovement;
        Climbing climb;

        Vector3 enterVel; 
        Rigidbody rb;
        Vector3 LedgePos;
        public float MaxLedgeHeight;
        public float MantleSpeed;
        public bool mantling;
        private void Start()
        {
            climb = GetComponent<Climbing>();
            playerMovement = GetComponent<PlayerMovement>();
            cam = GetComponent<HandleCam>().PC.GetComponent<Camera>();
            rb = GetComponent<Rigidbody>();
        }


        private void OnCollisionEnter(Collision collision)
        {
            Vector3 normal = collision.GetContact(0).normal;
            Vector3 horForward = cam.transform.forward;
            horForward.y = 0; 
            horForward.Normalize();
            if(Vector3.Angle(horForward, -normal)<= 45)
            {
                bool ledgeAvailable = true;
                RaycastHit hit;
                if(Physics.Raycast(cam.transform.position + MaxLedgeHeight*Vector3.up, -normal, out hit, 1, LayerMask.GetMask("Ground")))
                {
                    ledgeAvailable = false;
                }
                if (ledgeAvailable)
                {
                    Vector3 currentPos = cam.transform.position + Vector3.up * MaxLedgeHeight + Vector3.down * 0.05f;
                    while (!Physics.Raycast(currentPos, -normal, out hit, 1, LayerMask.GetMask("Ground")))
                    {
                        currentPos += Vector3.down * 0.05f;
                        if(currentPos.y < cam.transform.position.y-2f) {
                            break;
                        }
                    }
                    mantling = true;
                    enterVel = rb.velocity;
                    climb.enabled = false;
                    climbTime = 0;
                    rb.isKinematic = true;
                    playerMovement.enabled = false;
                    LedgePos = currentPos;
                }

            }

        }

        private void Update()
        {
            if (mantling)
            {
                DoMantle(LedgePos);

            }
        }

        float climbTime;

        public void DoMantle(Vector3 ledgePos)
        {

            Vector3 climbPos = ledgePos + 2 * Vector3.up + new Vector3(rb.velocity.x * climbTime, 0, rb.velocity.z * climbTime);

            rb.MovePosition(Vector3.Lerp(ledgePos, climbPos, climbTime));
            climbTime = Mathf.Clamp01(climbTime + Time.deltaTime * MantleSpeed);
            if (transform.position.y >= climbPos.y)
            {
                rb.velocity = enterVel;
                mantling = false;
                playerMovement.enabled = true;
                climb.enabled = false;
                rb.isKinematic = false;
                LedgePos = Vector3.zero;
            } 
        }

    }
}
