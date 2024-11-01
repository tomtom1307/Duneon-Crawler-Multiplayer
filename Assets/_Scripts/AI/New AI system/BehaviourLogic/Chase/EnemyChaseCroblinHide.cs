using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Chase-CroblinHide", menuName = "Enemy Logic/ Chase Logic/ CroblinHide")]
    public class EnemyChaseCroblinHide : EnemyChaseSOBase
    {
        public float HideTime;
        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            //base.DoFrameUpdateLogic();


        }

        public override void DoPhysicsUpdateLogic()
        {
            base.DoPhysicsUpdateLogic();
        }

        public override void Initialize(GameObject gameObject, NavMeshEnemy enemy)
        {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }

    }
}
