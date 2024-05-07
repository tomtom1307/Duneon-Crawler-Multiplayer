using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SecondaryAttack : WeaponComponent<SecondaryDamageData, SecondaryAttackDamage>
    {
        private ActionSphereOverlap sphereDetect;
        TakeDamage TD;

        private void HandleColliderDetection(Collider collider)
        {
            if (collider.tag == "Head")
            {
                return;
            }
            else
            {
                TD = collider.GetComponent<TakeDamage>();
                TD.DoDamageServerRpc(currentAttackData.DamageAmount);
                if(collider.GetComponent<Rigidbody>() != null)
                {
                    TD.DisableNavMeshServerRpc();
                    TD.KnockBackServerRpc(Camera.main.transform.position, currentAttackData.KnockBackAmount);
                }
                
            }


        }

        protected override void Awake()
        {
            base.Awake();

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
