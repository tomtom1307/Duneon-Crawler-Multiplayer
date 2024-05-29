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
            Debug.Log(enemy);
            enemy.target = enemy.DetectPlayer();
            enemy.animator.SetBool("Attacking", false);
            _targetPos = enemy.target.position + enemy.AttackDistance * (enemy.transform.position - enemy.target.position).normalized;
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

        public override void ResetValues()
        {
            base.ResetValues();
        }
    }
}
