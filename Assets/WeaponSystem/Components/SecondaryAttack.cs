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
                print(ChargePercentage + "+" + currentAttackData.DamageAmount);
                TD.DoDamageServerRpc(ChargePercentage * currentAttackData.DamageAmount);
                if(collider.GetComponent<Rigidbody>() != null)
                {
                    TD.DisableNavMeshServerRpc();
                    TD.KnockBackServerRpc(Camera.main.transform.position, ChargePercentage*currentAttackData.KnockBackAmount);
                }
                
            }


        }

        protected override void Start()
        {
            base.Start();
            PA = GetComponent<PlayerAttack>();
            sphereDetect = GetComponent<ActionSphereOverlap>();
            sphereDetect.OnDetectedColliders += HandleColliderDetection;

        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            sphereDetect.OnDetectedColliders -= HandleColliderDetection;
        }
    }
}
