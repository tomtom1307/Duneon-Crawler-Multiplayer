using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Damage : WeaponComponent<DamageData, AttackDamage>
    {
        private ActionRayCast rayCast;
        Enemy TD;
        
        private void HandleColliderDetection(Collider collider)
        {
            bool isMagic;
            

            if (currentAttackData.type == AttackDamage.DamageType.Magic) isMagic = true;
            else isMagic = false;

            if(collider.tag == "Damageable")
            {
                DamageableThing DT = collider.GetComponent<DamageableThing>();
                DT.TakeDamageServerRpc(weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic));
            }

            else if(collider.tag == "Projectile")
            {
                return;
            }

            else if (collider.tag == "Head")
            {
                weapon._soundSource.PlaySound(SourceSoundManager.SoundType.HeadShotDink, 2f);
                weapon.statManager.GiveWeaponXP(5, weapon.statManager.weaponXPscaling);
                print("Head");
                TD = collider.GetComponentInParent<Enemy>();
                weapon.statManager.GiveXP(weapon.statManager.headShotXP);
                
                TD.DoDamageServerRpc(weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic), true);

                
                
                if (collider.GetComponent<Rigidbody>() != null)
                {
                    //if (!TD.navMesh.enabled)
                    //{
                        TD.KnockBackServerRpc(Camera.main.transform.position, currentAttackData.KnockBackAmount);
                    //}
                }
                
            }
            else
            {
                weapon.statManager.GiveWeaponXP(3, weapon.statManager.weaponXPscaling);
                TD = collider.GetComponent<Enemy>();
                if(TD == null)
                {
                    TD = collider.GetComponentInParent<Enemy>();    
                }
                TD.DoDamageServerRpc(weapon.statManager.GetDamageVal(currentAttackData.DamageAmount, isMagic));



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
