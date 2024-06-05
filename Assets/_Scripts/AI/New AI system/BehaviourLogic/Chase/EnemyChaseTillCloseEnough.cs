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
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
            Debug.Log("Chasing");
            enemy.target = enemy.DetectPlayer();
            enemy.animator.SetBool("Attacking", false);
            Vector3 randomPos = new Vector3(Random.Range(-1f,1f),0, Random.Range(-1f, 1f)).normalized;
            _targetPos = enemy.target.position +  enemy.AttackDistance * (randomPos + enemy.transform.position - enemy.target.position).normalized;
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            _targetPos = enemy.target.position;
            enemy.MoveEnemy(_targetPos);
        }

        public override void Initialize(GameObject gameObject, Enemy enemy)
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
