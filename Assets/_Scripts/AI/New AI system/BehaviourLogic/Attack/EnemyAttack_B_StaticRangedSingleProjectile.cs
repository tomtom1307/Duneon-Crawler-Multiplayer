using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class EnemyAttackStaticRangedSingleProjectile : EnemyAttackSOBase
    {
        public GameObject Projectile;

        public float timeBetweenAttacks = 2f;
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
            
            if(enemy.target == null)
            {
                return;
            }
            _timer += Time.deltaTime;
            if (_timer > timeBetweenAttacks)
            {
                _timer = 0;
                enemy.Attacking = true;
                Vector3 ArrowDir = enemy.target.position - transform.position;
                //enemy.SpawnObj(Projectile, enemy.ProjectileSpawnPos.position);

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
