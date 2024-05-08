using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SecondaryAttack : WeaponComponent<SecondaryDamageData, SecondaryAttackDamage>
    {
        private ActionSphereOverlap sphereDetect;
        TakeDamage TD;
        public float ChargePercentage;
        PlayerAttack PA;

        private void HandleColliderDetection(Collider collider)
        {
            ChargePercentage = PA.ChargePercentage;
            if (collider.tag == "Head")
            {
                return;
            }
            else
            {
                TD = collider.GetComponent<TakeDamage>();
                TD.DoDamageServerRpc(ChargePercentage * currentAttackData.DamageAmount);
                if(collider.GetComponent<Rigidbody>() != null)
                {
                    TD.DisableNavMeshServerRpc();
                    TD.KnockBackServerRpc(Camera.main.transform.position, ChargePercentage*currentAttackData.KnockBackAmount);
                }
                
            }


        }

        protected override void Awake()
        {
            base.Awake();
            PA = GetComponent<PlayerAttack>();
            sphereDetect = GetComponent<ActionSphereOverlap>();

        }

        protected override void OnEnable()
        {
            base.OnEnable();

            sphereDetect.OnDetectedColliders += HandleColliderDetection;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            sphereDetect.OnDetectedColliders -= HandleColliderDetection;
        }
    }
}
