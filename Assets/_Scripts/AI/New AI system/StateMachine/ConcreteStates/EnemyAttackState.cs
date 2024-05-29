using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

namespace Project
{
    public class EnemyAttackState : EnemyState
    {

        
        public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);

            enemy.EnemyAttackInstance.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();

            enemy.EnemyAttackInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            enemy.EnemyAttackInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            enemy.EnemyAttackInstance.DoFrameUpdateLogic();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            enemy.EnemyAttackInstance.DoPhysicsLogic();
        }
    }
}
