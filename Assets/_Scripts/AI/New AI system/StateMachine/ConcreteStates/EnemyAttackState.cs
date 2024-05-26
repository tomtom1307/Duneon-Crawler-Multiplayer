using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

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
        bool attacking = false;
        Vector3 DesiredPosition;

        public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);

            if (triggerType == Enemy.AnimationTriggerType.FinishedAttacking)
            {
                attacking = false;
                enemy.animator.SetBool("Attacking", false);
                if (enemy.aggression < 0.8)
                {
                    Reposition();
                }
            }
            else if (triggerType == Enemy.AnimationTriggerType.CallHit)
            {
                enemy.Attack();
            }
            
        }

        public override void EnterState()
        {
            base.EnterState();
            attacking = false;
            enemy.navMesh.stoppingDistance = 0;
            timeBetweenAttacks = enemy.TimeBetweenAttacks;
            _timer = timeBetweenAttacks*0.8f;
            //TODO IMPROVE THE SELECTION OF THE POSITION
            Debug.Log("Entered AttackState");
            if(enemy.aggression < 0.8f)
            {
                Reposition();
            }
            
            
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
                Debug.Log("Back To Chasing");
            }

            if(_timer > (timeBetweenAttacks - Mathf.Clamp((enemy.aggression),0.1f,1))&& !repositioning && !attacking) 
            {
                attacking = true;
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
                enemy.transform.DOMove(enemy.transform.position + enemy.lungeDistance * dir, 0.5f);


            }

            

            else if (repositioning)
            {
                if (Vector3.Distance(DesiredPosition, enemy.transform.position) < 1)
                {
                    Debug.Log("FinishedRepositioning");
                    repositioning = false;
                }
            }
            
        }

        

        public void Reposition()
        {
            if (attacking) return;
            Debug.Log("Repositioning");
            repositioning = true;
            DesiredPosition = RandomPosAroundPlayer(enemy.target.position, enemy.AttackDistance*0.7f);
            enemy.MoveEnemy(DesiredPosition);
        }

        



        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public Vector3 RandomPosAroundPlayer(Vector3 playerPos, float Radius)
        {
            //THIS NEEDS WORK
            Vector3 directionWithRandom = Radius * ((enemy.transform.position-playerPos + (Vector3)Random.insideUnitCircle).normalized);
            Vector3 randomVec = new Vector3(directionWithRandom.x, 0, directionWithRandom.z);
            return randomVec + playerPos; 
        }

    }
}
