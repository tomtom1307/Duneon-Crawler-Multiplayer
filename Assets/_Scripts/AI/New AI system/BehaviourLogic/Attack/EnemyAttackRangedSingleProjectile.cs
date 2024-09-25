using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-SingleRanged", menuName = "Enemy Logic/ Attack Logic/ Single Projectile logic")]
    public class EnemyAttackRangedSingleProjectile : EnemyAttackSOBase
    {
        public GameObject Projectile;
        private float _timer;

        public float timeBetweenAttacks = 2f;
        bool repositioning = false;
        Vector3 DesiredPosition;
        Vector3 OffsetVector;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);

            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                Debug.Log("FinishedAttacking");
                enemy.Attacking = false;
                enemy.animator.SetBool("Attacking", false);
                _timer = 0;
                Reposition();
            }
            else if (type == Enemy.AnimationTriggerType.CallHit)
            {
                enemy.Attack();
            }
            else if (type == Enemy.AnimationTriggerType.SpawnProjectile)
            {
                enemy.SpawnObj(Projectile, enemy.ProjectileSpawnPos.position);
            }

        }

        public override void DoEnterLogic()
        {
           
            Debug.Log("Attacking");
            base.DoEnterLogic();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        Vector3 preference;

        public override void DoFrameUpdateLogic()
        {
            base.DoFrameUpdateLogic();
            if (!enemy.Attacking)
            {
                DesiredPosition = enemy.target.position + OffsetVector;
                enemy.MoveEnemy(DesiredPosition);
            }
            enemy.LookAtTarget();
            _timer += Time.deltaTime;

            if (!enemy.IsWithinStrikingDistance)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
                Debug.Log("Back To Chasing");
            }


            //CHECK BETWEEN PLAYER AND THIS ENEMY FOR ANY OTHER ENEMIES IF TRUE THEN REPOSITION!!!!!!
            if (!enemy.CheckLOS(out preference))
            {
                Reposition(preference);
                return;
            }


            if (_timer > (timeBetweenAttacks - Mathf.Clamp((enemy.aggression), 0.1f, 1)) && !enemy.Attacking && enemy.navMesh.isActiveAndEnabled)
            {
                _timer = 0;
                enemy.navMesh.SetDestination(transform.position);
                enemy.animator.SetBool("Attacking", true);
                enemy.Attacking = true;
                Vector3 ArrowDir = enemy.target.position - transform.position;

            }



            else if (repositioning)
            {
                if (Vector3.Distance(DesiredPosition, enemy.transform.position) < 0.6f)
                {
                    Debug.Log("FinishedRepositioning");
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
            enemy.animator.SetBool("Attacking", false);
        }

        public void Reposition()
        {
            if (enemy.Attacking)
            {
                enemy.navMesh.SetDestination(transform.position);
                return;
            } 
            Debug.Log("Repositioning");
            repositioning = true;
            OffsetVector = RandomPosAroundPlayer(enemy.AttackDistance);

        }

        public void Reposition(Vector3 Dir)
        {
            if (enemy.Attacking) return;
            Dir = Dir.normalized;
            Debug.Log("Repositioning");
            repositioning = true;
            OffsetVector = preferedPositionToPlayer(enemy.AttackDistance, Dir);

        }



        public Vector3 RandomPosAroundPlayer(float Radius)
        {
            //THIS NEEDS WORK
            Vector3 directionWithRandom = Random.Range(-1f, 1f) * enemy.transform.right - enemy.transform.forward;
            Vector3 randomVec = new Vector3(directionWithRandom.x, 0, directionWithRandom.z).normalized;
            return Radius * randomVec;
        }

        public Vector3 preferedPositionToPlayer(float Radius, Vector3 dirAwayFromEnemy)
        {

            Vector3 DirVec = Mathf.Sign(Vector3.Dot(transform.right, dirAwayFromEnemy)) * enemy.transform.right;

            return Radius * DirVec;
        }

    }
}
