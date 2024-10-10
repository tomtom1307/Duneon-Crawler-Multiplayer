using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-Box", menuName = "Enemy Logic/ Attacks/ TestBoxMelee")]
    public class Enemy_Attack_Melee_Box : Enemy_Attack_Melee_Base
    {

        public override void Detect(Enemy_Attack_ColliderDetector colliderDetector)
        {
            Detect_Player_Collider(colliderDetector);
        }

        public override void StopDetecting(Enemy_Attack_ColliderDetector colliderDetector)
        {
            DeactivateAttackCollider(colliderDetector);
        }


        public void DoAttack()
        {

        }
    }
}
