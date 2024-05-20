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
            
            bool isMagic;


            if (weapon.Data.Attack2Type == DamageType.Magic) isMagic = true;
            else isMagic = false;

            ChargePercentage = PA.ChargePercentage;

            


            if (collider.tag == "Head")
            {
                return;
            }
            else
            {
                
                TD = collider.GetComponent<TakeDamage>();
                TD.DoDamageServerRpc(ChargePercentage * weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic));
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
