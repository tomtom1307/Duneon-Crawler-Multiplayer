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
            eventHandler.OnAttackAction += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            weapon._soundSource.PlaySound(SourceSoundManager.SoundType.Attack1, 1.5f);
            Ray ray = new Ray(cam.transform.forward, cam.transform.position);
            RaycastHit hit;
            //Debug.Log("Handle Attack Action");
            Physics.SphereCast(cam.transform.position, data.detectionRadius, cam.transform.forward, out hit, currentAttackData.Distance, data.DetectableLayers);
            detected = hit.collider;
            
            if (detected == null)
            {
                var target = cam.transform.position + cam.transform.forward * currentAttackData.Distance;
                
                if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentAttackData.Distance, weapon.visualAttacks.onTouch))
                {
                    target = hit.point;
                }
                weapon.visualAttacks.ProjectileServerRpc(target);
                
                return;
            }
            weapon.visualAttacks.ProjectileServerRpc(hit.point);
            weapon.visualAttacks.VisualInfo.reachedtarget += ReachedTarget;
            
            if (detected.tag == "Head")
            {
                Headshot = true;
            }
            else Headshot = false;

            

        }

        public void ReachedTarget()
        {
            if (detected == null)
            {
                return;
            }
            OnDetectedCollider?.Invoke(detected);
        }

        
 

        protected override void OnDestroy()
        {
            base.OnDestroy();

            eventHandler.OnAttackAction -= HandleAttackAction;
        }


    }
}
