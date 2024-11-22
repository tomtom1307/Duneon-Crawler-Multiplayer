using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-StaticPredictiveRanged", menuName = "Enemy Logic/ Attack Logic/ Static Predictive Projectile logic")]
    public class EnemyAttackStaticRangedPredictiveProjectile : EnemyAttackSOBase
    {
        public Enemy_Attack attack;
        public float timeBetweenAttacks = 2f;
        public float PredictionFactor;
        bool repositioning = false;
        Vector3 DesiredPosition;
        Vector3 OffsetVector;


        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            _timer = Random.Range(0f, 2f);
            Debug.Log("Attacking");
            base.DoEnterLogic();
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            Debug.Log(enemy.target);
            if(enemy.target == null)
            {
                enemy.FindPlayers();
                enemy.target = enemy.DetectPlayer();
                return;
            }
            _timer += Time.deltaTime;
            if (_timer > timeBetweenAttacks)
            {
                _timer = 0;
                enemy.Attacking = true;
                

                
                //Implement arc calculation
                Vector3 ShootDir = enemy.target.position - transform.position+ PredictionFactor*enemy.target.gameObject.GetComponent<Rigidbody>().velocity;
                enemy.TriggerAttack(attack);

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
