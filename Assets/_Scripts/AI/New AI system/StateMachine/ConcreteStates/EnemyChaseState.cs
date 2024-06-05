using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    public class EnemyChaseState : EnemyState
    {
        
        public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);

            enemy.EnemyChaseInstance.DoAnimationTriggerEventLogic(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            
            enemy.EnemyChaseInstance.DoEnterLogic();
        }

        public override void ExitState()
        {
            base.ExitState();

            enemy.EnemyChaseInstance.DoExitLogic();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            
            enemy.EnemyChaseInstance.DoFrameUpdateLogic();

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            enemy.EnemyChaseInstance.DoPhysicsUpdateLogic();
        }

        
    }
}
