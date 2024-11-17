using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_Ranged_Base : Enemy_Attack
    {
        public GameObject ProjectileModel;
        public float ProjectileSpeed;
        public bool Gravity;


        public override void Detect(Enemy enemy)
        {
            if (enemy.LOS(enemy.currentPlayer.transform.position))
            {
                return;
            }
            //Cancel animation???


        }

        

        
    }
}
