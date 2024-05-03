using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ActionRayCast : WeaponComponent<ActionRayCastData, AttackActionRayCast>
    {

        private Camera cam;
        private Collider detected;
        public bool Headshot;
        public event Action<Collider> OnDetectedCollider;

        protected override void Start()
        {
            
            base.Start();
            cam = Camera.main;
        }

        private void HandleAttackAction()
        {
            
            Ray ray = new Ray(cam.transform.forward, cam.transform.position);
            RaycastHit hit;
            //Debug.Log("Handle Attack Action");
            Physics.SphereCast(cam.transform.position, data.detectionRadius, cam.transform.forward, out hit, currentAttackData.Distance, data.DetectableLayers);
            detected = hit.collider;
            if (detected == null)
            {
                return;
            }

            OnDetectedCollider?.Invoke(detected);
            if (detected.tag == "Head")
            {
                Headshot = true;
            }
            else Headshot = false;


        }


        protected override void OnEnable()
        {
            base.OnEnable();

            eventHandler.OnAttackAction += HandleAttackAction;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            eventHandler.OnAttackAction -= HandleAttackAction;
        }


    }
}
