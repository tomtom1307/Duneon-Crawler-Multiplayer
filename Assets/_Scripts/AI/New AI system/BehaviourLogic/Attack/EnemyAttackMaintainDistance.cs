using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-Lunge", menuName = "Enemy Logic/ Attack Logic/ Melee-Lunge")]
    public class EnemyAttackMaintainDistance : EnemyAttackSOBase
    {
        [field: SerializeField] public float lungeDistance { get; set; } = 4f;

        private float _timer;

        private float timeBetweenAttacks = 2f;
        private float exitTimer;
        private float timeTillRetreat = 1;
        private float distanceTillRetreat;
        bool repositioning = false;
        Vector3 DesiredPosition;
        Vector3 OffsetVector;


        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);

            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                enemy.Attacking = false;
                enemy.animator.SetBool("Attacking", false);
                Reposition();
            }
            else if (type == Enemy.AnimationTriggerType.CallHit)
            {
                enemy.Attack();
            }

        }

        public override void DoEnterLogic()
        {
            //Debug.Log("Attacking");
            //enemy.navMesh.velocity = Vector3.zero;
            base.DoEnterLogic();
            Reposition();
           
        }

        public override void DoExitLogic()
        {
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
                enemy.MoveEnemy(DesiredPosition);
            }
            enemy.LookAtTarget();
            _timer += Time.deltaTime;



            //CHECK BETWEEN PLAYER AND THIS ENEMY FOR ANY OTHER ENEMIES IF TRUE THEN REPOSITION!!!!!!
            if (!enemy.CheckLOS(out preference))
            {
                Reposition(preference);
                return;
            }


            if (_timer > (timeBetweenAttacks) && !enemy.Attacking)
            {
                if (!enemy.navMesh.enabled) return;
                enemy.Attacking = true;
                _timer = Random.Range(0f,2f);
                //Debug.Log("Attacking");
                Vector3 dir = enemy.target.position - enemy.transform.position;
                if(dir.magnitude > lungeDistance)
                {
                    dir = lungeDistance*dir.normalized;
                }
                //dir = dir.normalized * lungeDistance;
                dir.y = 0;
                enemy.animator.SetBool("Attacking", true);
                
                enemy.navMesh.SetDestination(enemy.transform.position + lungeDistance * dir);
                enemy.transform.DOMove(enemy.transform.position + 0.8f * dir, 0.2f);

                
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
            //Debug.Log("Repositioning");
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
            Vector3 directionWithRandom = Mathf.Sign(Random.Range(-1f,1f))* enemy.transform.right - enemy.transform.forward;
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
