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
        public float ChargePercentage;
        PlayerAttack PA;

        protected override void Start()
        {
            
            base.Start();
            PA = GetComponent<PlayerAttack>();
            cam = Camera.main;
        }

        private void HandleAttackAction()
        {
            ChargePercentage = PA.ChargePercentage;
       
            //Debug.Log("Handle Attack Action");
            detected = Physics.OverlapSphere(cam.transform.position, data.detectionRadius* ChargePercentage, data.DetectableLayers);

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
