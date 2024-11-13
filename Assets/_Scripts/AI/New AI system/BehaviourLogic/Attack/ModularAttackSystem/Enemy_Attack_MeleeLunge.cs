using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Attack-Melee-Lunge", menuName = "Enemy Logic/ Attacks/ Melee-Lunge-Attack")]
    public class Enemy_Attack_MeleeLunge : Enemy_Attack_Melee_Base
    {

        public float LungeDistance;
        public float LungeSpeed;
        public override void Attack(PlayerStats ps, Enemy enemy)
        {
            base.Attack(ps, enemy);

            Vector3 dir = ps.transform.position - enemy.transform.position;
            dir.y = 0;
            
            if (Vector3.Distance(ps.transform.position, enemy.transform.position) > LungeDistance)
            {
                dir = dir.normalized;
            }
            
            
            enemy.transform.DOMove(enemy.transform.position + dir * LungeDistance, LungeSpeed).SetEase(Ease.OutQuint);
            
        }

        public override void Detect(Enemy_Attack_ColliderDetector detector)
        {
            base.Detect(detector);
        }

        public override void Detect(Enemy enemy)
        {
            base.Detect(enemy);
        }

        public override void playerHitLogic(PlayerStats ps, Enemy enemy)
        {
            base.playerHitLogic(ps, enemy);
        }

        public override void StopDetecting(Enemy_Attack_ColliderDetector detector)
        {
            base.StopDetecting(detector);
        }

        public override void StopDetecting(Enemy enemy)
        {
            base.StopDetecting(enemy);
        }
    }
}
