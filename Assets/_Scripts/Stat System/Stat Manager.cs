using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class StatManager : MonoBehaviour
    {
        public PlayerStats stats;

        [Header("Inherited from player")]
        public float MagicDamage;
        public float PhysicalDamage;
        public float AttackSpeed;
        public AnimationCurve weaponXPscaling;
        [Header("Scaling")]
        public int headShotXP;
        public float AttackSpeedScale;
        public float DamageScale;
        float physicalBonus = 0;
        float magicBonus = 0;
        // Start is called before the first frame update

        public WeaponInstance weaponInstance;

        void Start()
        {
           
        }

        // Update is called once per frame
        public void InitDamageVals()
        {
            if (stats == null) return;
            if (weaponInstance != null)
            {
                physicalBonus = weaponInstance.currentPhysicalDamageBonus;
                magicBonus = weaponInstance.currentMagicDamageBonus;
            }
            else
            {
                physicalBonus = 0;
                magicBonus = 0;
            }
            print(magicBonus);
            MagicDamage = stats.Intelligence * magicBonus/100;
            PhysicalDamage = stats.Strength *  physicalBonus/100;
            AttackSpeed = stats.Dexterity * AttackSpeedScale;
        }

        public float GetDamageVal(float BaseWeaponDamage,bool Magic = false)
        {
            if (stats == null) return 1;
            if (weaponInstance != null)
            {
                physicalBonus = weaponInstance.currentPhysicalDamageBonus;
                magicBonus = weaponInstance.currentMagicDamageBonus;
            }
            else
            {
                physicalBonus = 0;
                magicBonus = 0;
            }
            print(magicBonus);
            MagicDamage = stats.Intelligence * (BaseWeaponDamage + magicBonus) / 100;
            PhysicalDamage = stats.Strength * (BaseWeaponDamage + physicalBonus)/ 100;
            AttackSpeed = stats.Dexterity * AttackSpeedScale;

            if(Magic) return MagicDamage;
            return PhysicalDamage;

        }

        public void GiveXP(int amount)
        {
            if (stats == null) return;

            stats.AddXp(amount);
        }

        public void GiveWeaponXP(int amount, AnimationCurve Scaling)
        {
            weaponInstance.AddWeaponXp(amount,Scaling);
        }
    }
}
