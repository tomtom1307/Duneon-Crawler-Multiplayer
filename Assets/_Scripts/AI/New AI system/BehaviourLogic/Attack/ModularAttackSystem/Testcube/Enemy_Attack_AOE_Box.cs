using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-Box", menuName = "Enemy Logic/ Attacks/ TestBox/ TestBoxAOE")]
    public class Enemy_Attack_AOE_Box : Enemy_Attack_AOE_Base
    {
        public override void Attack(PlayerStats ps, Enemy enemy)
        {

        }

        public override void playerHitLogic(PlayerStats ps, Enemy enemy)
        {
            DoKnockBack(ps, enemy);
        }
    }
}
