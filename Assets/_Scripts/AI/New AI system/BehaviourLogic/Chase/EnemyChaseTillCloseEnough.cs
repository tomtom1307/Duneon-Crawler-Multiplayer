using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Chase-CloseDistance", menuName = "Enemy Logic/ Chase Logic/ Close Distance")]
    public class EnemyChaseTillCloseEnough : EnemyChaseSOBase
    {

        private Vector3 _targetPos;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                enemy.Attacking = false;
                enemy.animator.SetBool("Attacking", false);

            }
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Debug.Log("Chasing Enter Logic");
            enemy.target = enemy.DetectPlayer();
            enemy.animator.SetBool("Attacking", false);
            Vector3 randomPos = new Vector3(Random.Range(-1f,1f),0, Random.Range(-1f, 1f)).normalized;
            _targetPos = enemy.target.position +  enemy.AttackDistance * (randomPos + enemy.transform.position - enemy.target.position).normalized;
            enemy.MoveEnemy(_targetPos);
        }

        public override void DoExitLogic()
        {
            enemy.MoveEnemy(transform.position);
            base.DoExitLogic();
        }


        float Timer;
        float RecalcPath = 0.6f;

        public override void DoFrameUpdateLogic()
        {
            Timer += Time.deltaTime;
            base.DoFrameUpdateLogic();
            Vector3 randomPos = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _targetPos = enemy.target.position + enemy.AttackDistance * (randomPos + enemy.transform.position - enemy.target.position).normalized;
            if (Timer > RecalcPath)
            {
                Timer = 0;
                Debug.Log(_targetPos);
                Debug.Log(enemy);

                enemy.MoveEnemy(_targetPos);
            }
            
        }

        public override void Initialize(GameObject gameObject, NavMeshEnemy enemy)
        {
            base.Initialize(gameObject, enemy);
        }

        public bool SetBool(bool value)
        {
            return value;
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}
