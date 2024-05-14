using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Damage : WeaponComponent<DamageData, AttackDamage>
    {
        private ActionRayCast rayCast;
        TakeDamage TD;
        
        private void HandleColliderDetection(Collider collider)
        {
            

            if (collider.tag == "Head")
            {
                print("Head");
                TD = collider.GetComponentInParent<TakeDamage>();
                TD.DoDamageServerRpc(currentAttackData.DamageAmount, true);
                TD.DisableNavMeshServerRpc();
                if (collider.GetComponent<Rigidbody>() != null)
                {
                    
                    TD.KnockBackServerRpc(Camera.main.transform.position, currentAttackData.KnockBackAmount);
                }
                
            }
            else
            {
                TD =collider.GetComponent<TakeDamage>();
                TD.DoDamageServerRpc(currentAttackData.DamageAmount);
                TD.DisableNavMeshServerRpc();
                if (collider.GetComponent<Rigidbody>() != null)
                {
                    
                    TD.KnockBackServerRpc(Camera.main.transform.position,  currentAttackData.KnockBackAmount);
                }
                    

            }
            
            
        }

        protected override void Start()
        {
            base.Start();
            rayCast = GetComponent<ActionRayCast>();
            rayCast.OnDetectedCollider += HandleColliderDetection;

        }


        protected override void OnDestroy()
        {
            base.OnDestroy();

            rayCast.OnDetectedCollider -= HandleColliderDetection;
        }
    }
}
