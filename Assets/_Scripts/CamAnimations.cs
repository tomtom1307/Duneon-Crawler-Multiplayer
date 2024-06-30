using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Project
{
    public class CamAnimations : MonoBehaviour
    {
        public float MaxRotationAngle = 2f;
        public float smooth = 1f;
        public PlayerMovement PM;

        private void Start()
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        private void Update()
        {

            if (PM == null)
            {
                GameObject[] GOs = GameObject.FindGameObjectsWithTag("Player");
                foreach (var item in GOs)
                {
                    item.TryGetComponent<PlayerMovement>(out PM);
                    if (PM != null) break;
                }
                return;
            }



            float DirChangeCheck = Vector3.Dot(PM.rb.velocity, PM.orientation.right);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, Mathf.Clamp(-6*DirChangeCheck* PM.rb.velocity.magnitude/PM.sprintSpeed,-MaxRotationAngle, MaxRotationAngle)), Time.deltaTime* smooth);

        }

    }
}
