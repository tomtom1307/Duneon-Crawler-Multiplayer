using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class EnemyChaseSOBase : ScriptableObject
    {
        protected Enemy enemy;
        protected Transform transform;
        protected GameObject gameObject;

        protected Transform target;

        public virtual void Initialize(GameObject gameObject, Enemy enemy)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            this.enemy = enemy;
        }

        public virtual void DoEnterLogic()
        {

        }

        public virtual void DoExitLogic()
        {

        }

        public virtual void DoFrameUpdateLogic()
        {
            if (enemy.IsWithinStrikingDistance)
            {
                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        }

        public virtual void DoPhysicsUpdateLogic() { }

        public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {

        }

        public virtual void ResetValues()
        {

        }



    }

}