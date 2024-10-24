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
        public float ChaosBonus = 0;
        float physicalBonus = 0;
        int rarityIndex = 0;

        float[] ChaosBonusRarityMultiplier;

        float magicBonus = 0;
        // Start is called before the first frame update

        public WeaponInstance weaponInstance;

        void Start()
        {
           
        }

        public void ChangeStats(WeaponInstance inst)
        {
            weaponInstance = inst;
            InitDamageVals();
        }

        // Update is called once per frame
        public void InitDamageVals()
        {
            if (stats == null) return;
            if (weaponInstance != null)
            {
                physicalBonus = weaponInstance.currentPhysicalDamageBonus;
                magicBonus = weaponInstance.currentMagicDamageBonus;
                ChaosBonus = weaponInstance.ChaosBonus;
                rarityIndex = weaponInstance.rarityIndex;
            }
            else
            {
                physicalBonus = 0;
                magicBonus = 0;
            }
            print(magicBonus);
            MagicDamage = stats.Intelligence * magicBonus/100;
            PhysicalDamage = stats.Strength *  physicalBonus/100;
            AttackSpeed = ((stats.ChaosDexterity + stats.Dexterity) / 30 + 1);
        }

        public float GetDamageVal(float BaseWeaponDamage,bool Magic = false)
        {
            AttackSpeed = ((stats.ChaosDexterity+stats.Dexterity)/ 30 + 1);
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
            MagicDamage = stats.Intelligence * (BaseWeaponDamage + magicBonus +  ChaosBonus) / 100;
            PhysicalDamage = stats.Strength * (BaseWeaponDamage + physicalBonus + ChaosBonus)/ 100;
            

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
