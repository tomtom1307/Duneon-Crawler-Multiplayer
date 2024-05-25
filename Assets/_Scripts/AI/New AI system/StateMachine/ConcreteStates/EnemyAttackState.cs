using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace Project
{
    public class EnemyAttackState : EnemyState
    {

        private float _timer;
        private float timeBetweenAttacks = 2f;
        private float exitTimer;
        private float timeTillRetreat = 1;
        private float distanceTillRetreat;
        bool repositioning = false;
        Vector3 DesiredPosition;

        public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            enemy.navMesh.stoppingDistance = 0;
            timeBetweenAttacks = enemy.TimeBetweenAttacks;
            _timer = timeBetweenAttacks/2;
            //TODO IMPROVE THE SELECTION OF THE POSITION
            Debug.Log("Entered AttackState");
            Reposition();
            
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            enemy.LookAtTarget();
            base.FrameUpdate();
            _timer += Time.deltaTime;            

            if (!enemy.IsWithinStrikingDistance)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }

            if(_timer > timeBetweenAttacks && !repositioning) 
            {
                _timer = 0;
                Debug.Log("Attacking");
                Vector3 dir = enemy.target.position - enemy.transform.position;
                if(dir.magnitude >= enemy.lungeDistance)
                {
                    dir = dir.normalized * enemy.lungeDistance;
                }
                dir.y = 0;
                enemy.animator.SetBool("Attacking", true);
                enemy.navMesh.SetDestination(enemy.transform.position + enemy.lungeDistance * dir);
                enemy.transform.DOMove(enemy.transform.position + enemy.lungeDistance * dir,0.5f);


            }
            
            //TODO IF TOO CLOSE REPOSITION

            if (repositioning)
            {
                if (Vector3.Distance(DesiredPosition, enemy.transform.position) < 1f)
                {
                    Debug.Log("FinishedRepositioning");
                    repositioning = false;
                }
            }
            
        }

        public void Attack()
        {

        }

        public void Reposition()
        {
            Debug.Log("Repositioning");
            repositioning = true;
            DesiredPosition = RandomPosAroundPlayer(enemy.target.position, enemy.AttackDistance*0.6f);
            enemy.MoveEnemy(DesiredPosition);
        }


        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public Vector3 RandomPosAroundPlayer(Vector3 playerPos, float Radius)
        {

            Vector3 directionWithRandom = Radius * (enemy.transform.position-playerPos + 0.2f*new Vector3(Random.Range(0f, 1f),0, Random.Range(0f, 1f))).normalized;
            Vector3 randomVec = new Vector3(directionWithRandom.x, 0, directionWithRandom.z);
            return randomVec + playerPos; 
        }

    }
}
