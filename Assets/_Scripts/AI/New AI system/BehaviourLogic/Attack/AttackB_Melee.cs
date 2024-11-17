using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-Lunge", menuName = "Enemy Logic/ Attack Logic/ Melee-Lunge")]
    public class AttackB_Melee : EnemyAttackSOBase
    {
        public Enemy_Attack_Melee_Base[] meleeAttacks;
        

        public float timeBetweenAttacks = 2f;
        bool repositioning = false;
        Vector3 DesiredPosition;
        Vector3 OffsetVector;


        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);

            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                _timer = Random.Range(0f, 1.5f);
                Debug.Log("AttackAnimationFinished!");
                Reposition();
            }
            

        }

        public override void DoEnterLogic()
        {
            Debug.Log("AttackEnterLogic");
            //Debug.Log("Attacking");
            //enemy.navMesh.velocity = Vector3.zero;
            base.DoEnterLogic();
            NMEnemy.navMesh.SetDestination(target.position);
            enemy.TriggerAttack(meleeAttacks[0]);
            enemy.Attacking = false;
            NMEnemy.navMesh.updateRotation = false;
           
        }

        public override void DoExitLogic()
        {
            NMEnemy.navMesh.updateRotation = true;
            base.DoExitLogic();
        }

        Vector3 preference;
        float RecalcPath = 0.6f;
        float pathTimer;
        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            if (!enemy.Attacking)
            {
                DesiredPosition = enemy.target.position + OffsetVector;
                
            }



            enemy.LookAtTarget();
            _timer += Time.deltaTime;



            


            if (_timer > (timeBetweenAttacks) && !enemy.Attacking)
            {
                if (!NMEnemy.navMesh.enabled) return;
                enemy.TriggerAttack(meleeAttacks[0]);
                NMEnemy.navMesh.SetDestination(target.position);
                
            }



            else if (repositioning)
            {
                if (Vector3.Distance(DesiredPosition, enemy.transform.position) < 0.1f)
                {
                    //Debug.Log("FinishedRepositioning");
                    repositioning = false;
                }
            }

        }

        public override void Initialize(GameObject gameObject, Enemy enemy)
        {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }

        public void Reposition()
        {
            if (enemy.Attacking) return;
            NMEnemy.MoveEnemy(DesiredPosition);
            Debug.Log("Repositioning");
            repositioning = true;
            OffsetVector = RandomPosAroundPlayer(enemy.AttackDistance);

        }

        public void Reposition(Vector3 Dir)
        {
            if (enemy.Attacking) return;
            Dir = Dir.normalized;
            //Debug.Log("Repositioning");
            repositioning = true;
            OffsetVector = preferedPositionToPlayer(enemy.AttackDistance, Dir);

        }



        public Vector3 RandomPosAroundPlayer(float Radius)
        {
            //THIS NEEDS WORK
            Vector3 directionWithRandom = Random.Range(-1f,1f)* enemy.transform.right - enemy.transform.forward;
            Vector3 randomVec = new Vector3(directionWithRandom.x, 0, directionWithRandom.z).normalized;
            return Radius * randomVec;
        }

        public Vector3 preferedPositionToPlayer(float Radius, Vector3 dirAwayFromEnemy)
        {

            Vector3 DirVec = Mathf.Sign(Vector3.Dot(transform.right, dirAwayFromEnemy))* enemy.transform.right;

            return Radius * DirVec;
        }

    }
}
