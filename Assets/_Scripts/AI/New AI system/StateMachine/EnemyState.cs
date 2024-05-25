using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class EnemyState
    {
        protected Enemy enemy;
        protected EnemyStateMachine EnemyStateMachine;


        public EnemyState(Enemy enemy , EnemyStateMachine enemyStateMachine)
        {
            this.enemy = enemy;
            this.EnemyStateMachine = enemyStateMachine;
        }

        public virtual void EnterState() { }

        public virtual void ExitState() { } 

        public virtual void FrameUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) { }

    }
}
