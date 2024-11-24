using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Ranged-BasicProjectile", menuName = "Enemy Logic/ Attacks/ Ranged-BasicProjectile")]
    public class Enemy_Attack_Ranged_BasicProjectile : Enemy_Attack_Ranged_Base
    {
        public override void Attack(PlayerStats ps, Enemy enemy)
        {
            base.Attack(ps, enemy);
            Debug.Log(ps);
            Vector3 dir = ps.transform.position - enemy.transform.position;
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
