using Project.Assets._Scripts.Stat_System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class PlayerStats : NetworkBehaviour
    {
        [Header("Levels")]
        public int xp;
        public int requiredXP;
        public int level = 1;
        public AnimationCurve LevelXPScaling;

        [Header("Stats")]
        public float MaxHealth;
        public float MaxMana;
        public float Armor;
        public float Strength;
        public float Intelligence;
        public float Dexterity;

        [Header("Currency")]
        public int ChaosPoints;
        public int Gold;
        public int SkillPoints; 


        public float health;

        public StatManager StatManager;

        private void Awake()
        {
            

        }


        //Initialization
        private void Start()
        {
            if (!IsOwner)
            {
   
                Destroy(this);
            }
            //Find StatManager
            StatManager = FindAnyObjectByType<StatManager>();
            print(StatManager);
            StatManager.stats = this;
            StatManager.InitDamageVals();

            //Initialize health
            health = MaxHealth;

            //Initialize level
            requiredXP = GetRequiredXP(level);

            //Init UI display
            DisplayStatsUI.Singleton.UpdateUIStat(level, DisplayStatsUI.Singleton.LevelVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)MaxHealth, DisplayStatsUI.Singleton.MaxHealthVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)MaxMana, DisplayStatsUI.Singleton.MaxManaVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Strength, DisplayStatsUI.Singleton.StrengthVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Intelligence, DisplayStatsUI.Singleton.IntelligenceVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Dexterity, DisplayStatsUI.Singleton.DexVal);
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);


        }


        //Handle Damage stuff
        [ContextMenu("Test Shit")]
        public void RefreshStats()
        {
            StatManager.InitDamageVals();
        }


        //Handle Health


        
        public void TakeDamage(float damage)
        {
            damage -= Armor;
            damage = Mathf.Clamp(damage, 0, MaxHealth - 1);

            health -= damage;
            if (health <= 0)
            {
                //Enter Reiviable state
                // in revivable state to fight for you life Create a bar that has a marker that bounces left and right and 
                // if the input is given at the right time the revive timer increases slightly to buy time for teammates to revive you
            }
        }


        //Handle Levels and XP


        public int GetRequiredXP(int level)
        {

            int requiredXP = (int)LevelXPScaling.Evaluate(level);
            return requiredXP;
        }

        public void LevelUp()
        {
            level++;
            SkillPoints++;
            DisplayStatsUI.Singleton.UpdateUIStat(level, DisplayStatsUI.Singleton.LevelVal);
            
            requiredXP = GetRequiredXP(level);
            xp = 0;
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);
        }

        public void AddXp(int xpToAdd)
        {
            if(xpToAdd > requiredXP)
            {
                xpToAdd -= requiredXP;
                LevelUp();
            }
            requiredXP -= xpToAdd;
            xp += xpToAdd;
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);
        }

    }
}
