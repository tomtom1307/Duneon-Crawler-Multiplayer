using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class WeaponSway : MonoBehaviour
    {
        public float swayMult;
        public float smooth;
        public float weaponWalkSwayAmp;
        public float weaponWalkSwayFreq;
        public float VeloctiyDependence;
        public float AccelDependence;
        public float LandingEffect;
        public float JumpSnap;
        float landedMult = 1;
        public PlayerMovement PM;

        Vector3 initPos;
        Vector3 VelPrev = Vector3.zero; 

        private void Start()
        {
            initPos = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if( PM == null ) {
                GameObject[] GOs = GameObject.FindGameObjectsWithTag("Player");
                foreach (var item in GOs)
                {
                    item.TryGetComponent<PlayerMovement>(out PM);
                    if (PM != null) break;
                }
                return;
            }

            float mouseX = Input.GetAxisRaw("Mouse X") * swayMult;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMult;

            Quaternion rotX = Quaternion.AngleAxis(-mouseY, Vector3.forward);
            Quaternion rotY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRot = rotX * rotY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, smooth * Time.deltaTime);


            Vector3 targetPos;

           
            Vector3 accel = (PM.rb.velocity - VelPrev)/Time.deltaTime;

            if (PM.grounded)
            {

                if (PM.landed)
                {
                    landedMult = LandingEffect;
                }
                else landedMult = 1;

                targetPos = initPos +  landedMult*PM.rb.velocity.magnitude * weaponWalkSwayAmp  * new Vector3(Mathf.Sin(weaponWalkSwayFreq * Time.time), -landedMult*Mathf.Abs(Mathf.Sin(weaponWalkSwayFreq* Time.time)),0) +  (1-landedMult)*AccelDependence * PM.relLandingVel  * new Vector3(0, 1, 0); ;
                

                
            }
            else
            {
                targetPos = initPos + PM.rb.velocity.y * VeloctiyDependence* new Vector3(0, 1, 0);
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, JumpSnap * landedMult* Time.deltaTime);

            VelPrev = PM.rb.velocity;
        }
    }
}
