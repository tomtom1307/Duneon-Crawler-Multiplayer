using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SphereOverlapDamage : WeaponComponent<SecondaryDamageData, SecondaryAttackDamage>
    {
        private ActionSphereOverlap sphereDetect;
        Enemy TD;
        public float ChargePercentage;
        PlayerAttack PA;

        private void HandleColliderDetection(Collider collider)
        {
            
            bool isMagic = true;

            Debug.Log("SphereOverLapDAMAGE");

            if (data.Chargable)
            {
                ChargePercentage = PA.ChargePercentage;
            }
            else {
                ChargePercentage = 1;
            }
            

            
            if(collider.tag == "Projectile")
            {
                return;
            }

            if (collider.tag == "Head")
            {
                return;
            }
            else
            {
                DamageableThing DT;

                if (collider.tag == "Damageable")
                {
                    DT = collider.GetComponent<DamageableThing>();
                    Debug.Log("Damageablething;");
                    if(DT != null )
                    {
                        DT.TakeDamageServerRpc(weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic));
                        weapon.VFXHandler.SpawnOnHitParticleFX(DT.transform);
                    }
                   
                }

                TD = collider.GetComponent<Enemy>();
                if(TD == null)
                {
                    TD = collider.GetComponentInParent<Enemy>();
                    if (TD == null) return;
                }
                TD.DoDamageServerRpc(ChargePercentage * weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic));
                weapon.VFXHandler.SpawnOnHitParticleFX(TD.transform);
                NavMeshEnemy NME;
                if (collider.TryGetComponent<NavMeshEnemy>(out NME))
                {
                    if (data.IgnoreStaggerHealth)
                    {
                        NME.DisableNavMeshServerRpc();
                    }
                    TD.KnockBackServerRpc(Camera.main.transform.position, ChargePercentage * currentAttackData.KnockBackAmount);
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
