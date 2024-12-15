using Project.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "WeaponInput", menuName = "Weapon/Input/ PrimaryHoldSecondaryCharge")]
    public class GnomeTreeStaffInput : WeaponInputsSO
    {
        
        public override void InputLogic(WeaponHolder weapon, PlayerAttack PA)
        {
            if (Input.GetMouseButtonUp(1))
            {
                weapon._soundSource.StopSound();
                weapon._soundSource.PlaySound(SourceSoundManager.SoundType.Attack2Release, 0.5f * PA.ChargePercentage);
                weapon.anim.SetBool("SecondaryRelease", true);

                PA.Invoke(nameof(PA.ResetCharge),0.2f);
                
            }

            else if (Input.GetMouseButton(1))
            {

                if (!(weapon.State == WeaponHolder.AttackState.active))
                {
                    weapon.Enter(2);
                    weapon._soundSource.PlaySound(SourceSoundManager.SoundType.Attack2Charge, 0.5f);
                    PA.fullyCharged = false;
                }
                

                if (PA.ChargePercentage >= 1 && !PA.fullyCharged)
                {
                    PA.fullyCharged = true;
                    weapon._soundSource.PlaySound(SourceSoundManager.SoundType.Attack2Charged, 0.5f);
                }
                if (!PA.fullyCharged)
                {
                    PA.ChargePercentage += (Time.deltaTime * PA.ChargeSpeed);
                    PA.ChargePercentage = Mathf.Clamp(PA.ChargePercentage, 0, 1);
                }
                
                //StartCoroutine("Cooldown");
            }




            else if (Input.GetMouseButton(0))
            {
                //StartCoroutine("Cooldown");
                weapon.Enter(1);

            }
        }
    }
}
