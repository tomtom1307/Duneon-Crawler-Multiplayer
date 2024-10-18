using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class EnemyAttackSOBase : ScriptableObject
    {
        protected Enemy enemy;
        protected Transform transform;
        protected GameObject gameObject;
        protected NavMeshEnemy NMEnemy;
        protected Transform target;
        
        public virtual void Initialize(GameObject gameObject, Enemy enemy)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            this.enemy = enemy;
            gameObject.TryGetComponent<NavMeshEnemy>(out NMEnemy);
        }

        public virtual void DoEnterLogic()
        {

        }

        public virtual void DoExitLogic()
        {

        }

        public virtual void DoFrameUpdateLogic()
        {
            if (!enemy.IsWithinStrikingDistance)
            {
                NMEnemy.StateMachine.ChangeState(NMEnemy.ChaseState);
            }
        }

        public virtual void DoPhysicsLogic() { }

        public virtual void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                Debug.Log("Finished Attacking");
                enemy.AttackExit();
            }
        }

        public virtual void ResetValues()
        {

        }



    }

}
