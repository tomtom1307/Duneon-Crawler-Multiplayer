using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_AOE_Base : Enemy_Attack
    {
        public float AttackRadius;


        public override void Detect(Enemy enemy)
        {
            enemy.DoOverlapSphere(AttackRadius);
            
        }
    }
}
