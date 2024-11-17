using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "AttackB-MindlessChasing", menuName = "Enemy Logic/ Attack Logic/MindlessChasing")]
    public class EnemyAttackMindlessChasing : EnemyAttackSOBase
    {
        public float timeBetweenAttacks = 2f;
        public List<Enemy_Attack> Attacks;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            target = NMEnemy.target;
            _timer = timeBetweenAttacks;
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            _timer += Time.deltaTime;
            NMEnemy.MoveEnemy(target.position);
            if(_timer > timeBetweenAttacks && !NMEnemy.Attacking)
            {
                enemy.TriggerAttack(Attacks[Random.Range(0, Attacks.Count)]);
                Debug.Log("GoblinBrute attack");
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
