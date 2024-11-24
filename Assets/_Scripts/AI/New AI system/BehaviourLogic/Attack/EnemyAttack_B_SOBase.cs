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
        [HideInInspector] public float _timer;
        
        public virtual void Initialize(GameObject gameObject, Enemy enemy)
        {
            this.gameObject = gameObject;
            transform = gameObject.transform;
            this.enemy = enemy;
            gameObject.TryGetComponent<NavMeshEnemy>(out NMEnemy);
            Debug.Log("NAVMESH ENEMY:", NMEnemy);
        }

        public virtual void DoEnterLogic()
        {
            if(NMEnemy != null) { target = NMEnemy.target; }
        }

        public virtual void DoExitLogic()
        {
            
        }

        public virtual void DoFrameUpdateLogic()
        {
            if (!enemy.IsWithinStrikingDistance && !enemy.Attacking)
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
                _timer = 0;
            }
            if(type == Enemy.AnimationTriggerType.SpawnVFX)
            {
                enemy.SpawnFX(enemy.currentAttack.VFX);
            }
        }

        public virtual void ResetValues()
        {

        }



    }

}
