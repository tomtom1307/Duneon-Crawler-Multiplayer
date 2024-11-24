using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-StaticRanged", menuName = "Enemy Logic/ Attack Logic/ Ranged/ Static Projectile logic")]
    public class EnemyAttackStaticRangedNewAttackSystem : EnemyAttackSOBase
    {
        public Enemy_Attack attack;
        public float timeBetweenAttacks = 2f;
        public float MaxAttackDistance;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            _timer = Random.Range(0f, 2f);
            Debug.Log("Attacking");
            base.DoEnterLogic();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            if(enemy.target == null)
            {
                enemy.FindPlayers();
                enemy.target = enemy.DetectPlayer();
                target = enemy.target;
                return;
            }
            _timer += Time.deltaTime;
            if (_timer > timeBetweenAttacks)
            {
                if (Vector3.Distance(transform.position, target.position) > MaxAttackDistance) return;
                _timer = 0;
                enemy.Attacking = true;

                //Implement arc calculation
                enemy.TriggerAttack(attack);
                
            }

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
