using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "AttackB-Melee-Box", menuName = "Enemy Logic/ Attack Logic/ TestBox")]
    public class EnemyAttackTestBox : EnemyAttackSOBase
    {
        public Enemy_Attack_Melee_Base Melee_Attack1;
        bool Attacking = false;

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
            if (Input.GetKeyDown(KeyCode.L) && !Attacking)
            {
                Attacking = true;
                
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
