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

        [Header("Scaling")]
        public int headShotXP;
        public float AttackSpeedScale;
        public float DamageScale;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        public void InitDamageVals()
        {
            if (stats == null) return;

            MagicDamage = stats.Intelligence * DamageScale;
            PhysicalDamage = stats.Strength * DamageScale;
            AttackSpeed = stats.Dexterity * AttackSpeedScale;
        }

        public void GiveXP(int amount)
        {
            if (stats == null) return;

            stats.AddXp(amount);
        }
    }
}
