using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class VelocityLines : MonoBehaviour
    {
        public float VelocityThreshold;
        PlayerMovement movement;
        ParticleSystem PS;

        void Start()
        {
            movement = GetComponentInParent<PlayerMovement>();
            PS = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {

            transform.rotation = Quaternion.LookRotation(-movement.rb.velocity) * Quaternion.Euler(new Vector3(90, 0, 0));

            if (movement.rb.velocity.magnitude > VelocityThreshold)
            {
                PS.startSpeed = movement.rb.velocity.magnitude;
                PS.enableEmission = true;
                

            }
            else PS.enableEmission = false;


        }
    }
}
