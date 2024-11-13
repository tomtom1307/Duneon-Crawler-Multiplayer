using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-FaceHug", menuName = "Enemy Logic/ Attacks/ Melee-FaceHug")]
    public class Enemy_Attack_FaceHug : Enemy_Attack_Melee_Base
    {
        public override void Attack(PlayerStats ps, Enemy enemy)
        {
            //base.Attack(ps, enemy);

            enemy.transform.parent = ps.transform;

            // Disable navmesh while croblin on face
        }

        public override void Detect(Enemy_Attack_ColliderDetector detector)
        {
            base.Detect(detector);
        }

        public override void Detect(Enemy enemy)
        {
            base.Detect(enemy);
        }

        public override void EnterLogic(Enemy enemy)
        {
            base.EnterLogic(enemy);
        }

        public override void ExitLogic(Enemy enemy)
        {
            base.ExitLogic(enemy);
            Debug.Log("It been exited");
        }

        public override void playerHitLogic(PlayerStats ps, Enemy enemy)
        {
            base.playerHitLogic(ps, enemy);
        }

        public override void StopDetecting(Enemy_Attack_ColliderDetector detector)
        {
            base.StopDetecting(detector);
        }

        public override void StopDetecting(Enemy enemy)
        {
            base.StopDetecting(enemy);
        }
    }
}
