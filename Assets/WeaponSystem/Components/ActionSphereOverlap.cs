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
            eventHandler.OnAOEAction += HandleAttackAction;
        }

        private void HandleAttackAction()
        {
            float manaUse;
            if (!weapon.statManager.stats.DoMagicAttack(weapon.Data.Attack2ManaUse, false))
            {
                print("Using All mana");

                manaUse = weapon.statManager.stats._mana.Value;
                ChargePercentage = manaUse / weapon.Data.Attack2ManaUse;
            }

            else manaUse = weapon.Data.Attack2ManaUse;
            if (data.Chargable)
            {
                ChargePercentage = PA.ChargePercentage;
            }
            else
            {
                ChargePercentage = 1;
            }
            print(weapon.visualAttacks);
            weapon.visualAttacks.AOEServerRpc(ChargePercentage);
            //Debug.Log("Handle Attack Action");
            detected = Physics.OverlapSphere(cam.transform.position, data.detectionRadius* ChargePercentage, data.DetectableLayers);
            weapon.statManager.stats.DoMagicAttack(manaUse * ChargePercentage);


            if (detected == null)
            {
                return;
            }

            foreach (Collider collider in detected)
            {
                
                if (collider.tag != "Head")
                {
                    OnDetectedColliders?.Invoke(collider);
                }
                
            }
            
            


        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            eventHandler.OnAOEAction -= HandleAttackAction;
        }


    }
}
