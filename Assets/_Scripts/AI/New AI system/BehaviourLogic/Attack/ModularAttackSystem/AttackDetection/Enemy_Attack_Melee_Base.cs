using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_Melee_Base : Enemy_Attack
    {
        public override void Detect(Enemy enemy)
        {
            enemy.EnableMeleeCollider();
        }

        public override void StopDetecting(Enemy enemy)
        {
            enemy.DisableMeleeCollider();
        }




    }
}
