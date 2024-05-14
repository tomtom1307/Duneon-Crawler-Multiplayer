using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.WSA;

namespace Project
{
    [CreateAssetMenu(fileName = "QuickStep", menuName ="Abilities/QuickStep")]
    public class QuickStep : Ability
    {
        // Start is called before the first frame update

        public float Acceleration;


        public override void Activate(GameObject parent, out bool fail)
        {
            
            PlayerMovement movement = parent.GetComponent<PlayerMovement>();
            if (!movement.grounded)
            {
                fail = true;
                return;
            }
            base.Activate(parent, out fail);
            Rigidbody rigidbody = parent.GetComponent<Rigidbody>();
            Transform orientation = movement.orientation;
            Vector3 DashDir = movement.horizontalInput * orientation.right + movement.verticalInput * orientation.forward;
            rigidbody.AddForce(Acceleration * DashDir,ForceMode.Impulse);
            
            
        }


    }
}
