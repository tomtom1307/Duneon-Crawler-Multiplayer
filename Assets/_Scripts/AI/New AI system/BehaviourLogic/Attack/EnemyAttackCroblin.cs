using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Croblin", menuName = "Enemy Logic/ Attack Logic/ Croblin")]
    public class EnemyAttackCroblin : EnemyAttackSOBase
    {
        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            NMEnemy.target = NMEnemy.DetectPlayer();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            //base.DoFrameUpdateLogic();
            NMEnemy.MoveEnemy(NMEnemy.target.position);
        }

        public override void DoPhysicsLogic()
        {
            base.DoPhysicsLogic();
        }

        public override void Initialize(GameObject gameObject, Enemy enemy)
        {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}
