using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Croblin", menuName = "Enemy Logic/ Attack Logic/ Croblin")]
    public class EnemyAttack_B_Croblin : EnemyAttackSOBase
    {
        public int attackType;

        public Enemy_Attack_Melee_Base[] meleeAttacks;
        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);

            if (type == Enemy.AnimationTriggerType.FinishedAttacking) {
                NMEnemy.transform.SetParent(null, true); //
                NMEnemy.StateMachine.ChangeState(NMEnemy.ChaseState);
                Debug.Log("EnterChaseState");
            }
        }

        public override void DoEnterLogic()
        {
            //NMEnemy.target = NMEnemy.DetectPlayer();
            base.DoEnterLogic();
            
            //Debug.Log(NMEnemy.target);
        }

        public override void DoExitLogic()
        {
            
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            //base.DoFrameUpdateLogic();
            NMEnemy.MoveEnemy(NMEnemy.target.position);

            if (NMEnemy.IsWithinStrikingDistance && !NMEnemy.Attacking)
            {
                attackType = Random.Range(1, 2);
                Debug.Log(attackType);
                Debug.Log(NMEnemy.target + "This is the current target of the croblin");

                NMEnemy.TriggerAttack(meleeAttacks[attackType]);

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
