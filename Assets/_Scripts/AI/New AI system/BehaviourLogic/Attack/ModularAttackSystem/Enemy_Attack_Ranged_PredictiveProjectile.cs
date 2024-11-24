using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Ranged-PredictiveProjectile", menuName = "Enemy Logic/ Attacks/ Ranged-PredictiveProjectile")]
    public class Enemy_Attack_Ranged_PredictiveProjectile : Enemy_Attack_Ranged_Base
    {
        public float PredictionFactor;
        public float DistanceGravityFactor;
        public override void Attack(PlayerStats ps, Enemy enemy)
        {
            base.Attack(ps, enemy);
            Debug.Log(ps);
            float Distance = Vector3.Distance(enemy.target.position, enemy.ProjectileSpawnPos.position);
            Vector3 effPos = (ps.transform.position + PredictionFactor * ps.GetComponent<Rigidbody>().velocity);
            Vector3 dir = effPos - enemy.ProjectileSpawnPos.position + Distance * DistanceGravityFactor * Vector3.up;
            enemy.SpawnProjectile(ProjectileModel, enemy.ProjectileSpawnPos.position, dir, ProjectileSpeed, Damage);
        }


        public override void Detect(Enemy enemy)
        {
            base.Detect(enemy);
        }

        public override void EnterLogic(Enemy enemy)
        {
            base.EnterLogic(enemy);
        }

        public override void ExitLogic(Enemy enemy)
        {
            base.ExitLogic(enemy);
        }

        public override void playerHitLogic(PlayerStats ps, Enemy enemy)
        {
            base.playerHitLogic(ps, enemy);
        }

        public override void StopDetecting(Enemy enemy)
        {
            base.StopDetecting(enemy);
        }

    }
}
