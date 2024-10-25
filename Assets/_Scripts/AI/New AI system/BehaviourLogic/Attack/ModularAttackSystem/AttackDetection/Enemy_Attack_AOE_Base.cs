using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_AOE_Base : Enemy_Attack
    {
        public float AttackRadius;
        public float KnockBackForce;

        public override void Detect(Enemy enemy)
        {
            enemy.DoOverlapSphere(AttackRadius);
            
        }

        public void DoKnockBack(PlayerStats ps, Enemy enemy)
        {
            Rigidbody rb = ps.GetComponent<Rigidbody>();
            Vector3 dir = ps.transform.position - enemy.transform.position;
            rb.AddForce(KnockBackForce * dir.normalized);
        }
    }
}
