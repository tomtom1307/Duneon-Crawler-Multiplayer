using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class WeaponInstance
    {
        public WeaponDataSO weaponData;
        public int level;
        public int xp;
        public AnimationCurve xpScaling;
        public int requiredXP;
        public int rarityIndex;
        public string rarity;
        
        // Add properties that might change with leveling
        public float currentMagicDamageBonus;
        public float currentPhysicalDamageBonus;

        public float[] rarityDamageBonus =
        {
            10,
            20,
            30,
            40,
            50
        };


        public string[] rarities =
        {
            "Common",
            "Uncommon",
            "Rare",
            "Very Rare",
            "Legendary"
        };

        public WeaponInstance(WeaponDataSO data, int RarityIndex)
        {
            weaponData = data;
            level = 1;
            xp = 0;
            this.rarity = rarities[RarityIndex];
            rarityIndex = RarityIndex;

            float RarityDamageBonus = rarityDamageBonus[rarityIndex];

            float RarityDamageBonusLowerBound;
            if (rarityIndex == 0) RarityDamageBonusLowerBound = 100;
            else RarityDamageBonusLowerBound = rarityDamageBonus[rarityIndex-1];

            currentPhysicalDamageBonus = UnityEngine.Random.Range(RarityDamageBonusLowerBound,RarityDamageBonus);
            currentMagicDamageBonus = UnityEngine.Random.Range(RarityDamageBonusLowerBound, RarityDamageBonus);
        }


        public int GetRequiredXP(int level)
        {
            int requiredXP = (int)xpScaling.Evaluate(level);
            return requiredXP;
        }

        public void LevelUp()
        {
            level++;
            requiredXP = GetRequiredXP(level);
            xp = 0;

        }

        public void AddWeaponXp(int xpToAdd)
        {
            Debug.Log("weapon XP added!!!!");
            
            if (xpToAdd >= requiredXP)
            {
                xpToAdd -= requiredXP;
                LevelUp();
            }
            requiredXP -= xpToAdd;
            xp += xpToAdd;
            
        }


        public void AddWeaponXpClient(int xpToAdd)
        {
            AddWeaponXp(xpToAdd);
        }

    }
}
