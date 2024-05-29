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
        bool attacking = false;
        Vector3 DesiredPosition;
        Vector3 OffsetVector;


        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);

            if (type == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                attacking = false;
                enemy.animator.SetBool("Attacking", false);
                if (enemy.aggression < 0.8)
                {
                    Reposition();
                }
            }
            else if (type == Enemy.AnimationTriggerType.CallHit)
            {
                enemy.Attack();
            }

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
            base.DoFrameUpdateLogic();
            if (!attacking)
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


            //TODO BEFORE ATTACKING CHECK BETWEEN PLAYER AND THIS ENEMY FOR ANY OTHER ENEMIES IF TRUE THEN REPOSITION!!!!!!
            if (_timer > (timeBetweenAttacks - Mathf.Clamp((enemy.aggression), 0.1f, 1)) && !attacking)
            {
                attacking = true;
                _timer = 0;
                Debug.Log("Attacking");
                Vector3 dir = enemy.target.position - enemy.transform.position;
                dir = dir.normalized * lungeDistance;
                dir.y = 0;
                enemy.animator.SetBool("Attacking", true);
                enemy.navMesh.SetDestination(enemy.transform.position + lungeDistance * dir);
                enemy.transform.DOMove(enemy.transform.position + lungeDistance * dir, 0.35f);


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
        }

        public void Reposition()
        {
            if (attacking) return;
            Debug.Log("Repositioning");
            repositioning = true;
            OffsetVector = RandomPosAroundPlayer(enemy.AttackDistance);

        }

        public Vector3 RandomPosAroundPlayer(float Radius)
        {
            //THIS NEEDS WORK
            Vector3 directionWithRandom = Random.Range(-1f,1f) * enemy.transform.right - enemy.transform.forward;
            Vector3 randomVec = new Vector3(directionWithRandom.x, 0, directionWithRandom.z).normalized;
            return Radius * randomVec;
        }

    }
}
