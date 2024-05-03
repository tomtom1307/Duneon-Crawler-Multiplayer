using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ActionSphereOverlap : WeaponComponent<SphereOverlapData, AttackOverlapSphere>
    {

        private Camera cam;
        private Collider[] detected;
        public event Action<Collider> OnDetectedColliders;

        protected override void Start()
        {
            
            base.Start();
            cam = Camera.main;
        }

        private void HandleAttackAction()
        {
            
       
            //Debug.Log("Handle Attack Action");
            detected = Physics.OverlapSphere(cam.transform.position, data.detectionRadius, data.DetectableLayers);

            if (detected == null)
            {
                return;
            }

            foreach (Collider collider in detected)
            {
                
                if(collider.tag != "Head")
                {
                    OnDetectedColliders?.Invoke(collider);
                }
                
            }
            
            


        }


        protected override void OnEnable()
        {
            base.OnEnable();

            eventHandler.OnAOEAction += HandleAttackAction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            eventHandler.OnAOEAction -= HandleAttackAction;
        }


    }
}
