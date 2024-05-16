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
            float manaUse;
            

            float StatMult;

            if (weapon.Data.Attack1Type == DamageType.Magic) StatMult = weapon.statManager.MagicDamage/100;

            else StatMult = weapon.statManager.PhysicalDamage;

            ChargePercentage = PA.ChargePercentage;

            if (!weapon.statManager.stats.DoMagicAttack(weapon.Data.Attack2ManaUse,false))
            {
                print("Using All mana");
                
                manaUse = weapon.statManager.stats._mana.Value;
                ChargePercentage = manaUse / weapon.Data.Attack2ManaUse;
            }

            else manaUse = weapon.Data.Attack2ManaUse;


            if (collider.tag == "Head")
            {
                return;
            }
            else
            {
                weapon.statManager.stats.DoMagicAttack( manaUse * ChargePercentage * StatMult);
                TD = collider.GetComponent<TakeDamage>();
                print("ManaUsed:"+ manaUse);
                TD.DoDamageServerRpc(ChargePercentage * currentAttackData.DamageAmount * StatMult);
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
