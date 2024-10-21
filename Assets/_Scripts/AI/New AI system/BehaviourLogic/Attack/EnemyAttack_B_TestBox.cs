using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "AttackB-Melee-Box", menuName = "Enemy Logic/ Attack Logic/ TestBox")]
    public class EnemyAttack_B_TestBox : EnemyAttackSOBase
    {
        
        public List<Enemy_Attack_Melee_Base> MeleeAttacks;
        public List<Enemy_Attack_AOE_Base> AoEAttacks;
        

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
            if (Input.GetKeyDown(KeyCode.L) && !enemy.Attacking)
            {
                enemy.TriggerAttack(MeleeAttacks[0]);
            }

            if (Input.GetKeyDown(KeyCode.P) && !enemy.Attacking)
            {
                enemy.TriggerAttack(MeleeAttacks[1]);
            }
            if(Input.GetKeyDown(KeyCode.R) && !enemy.Attacking)
            {
                enemy.TriggerAttack(AoEAttacks[0]);
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
