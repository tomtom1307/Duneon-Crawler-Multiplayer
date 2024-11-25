using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Project
{
    public class Croblin : NavMeshEnemy
    {
        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            EnemyChaseInstance.Initialize(gameObject, this);


            StateMachine.Initialize(AttackState);
        }

        public void ChooseCollider(PlayerStats ps)
        {
            EnemyAttack_B_Croblin Croblin = EnemyAttackInstance as EnemyAttack_B_Croblin;

            if (Croblin != null)
            {
                if (Croblin.attackType == 1)
                {
                    HugPlayer(ps);
                }
                else
                {
                    DamagePlayer(ps);
                }
            }
        }
        public override void GetColliders()
        {
            colliders = GetComponentsInChildren<Collider>().ToList();
            colliders.Add(GetComponent<Collider>());

            colliderDetector = GetComponentsInChildren<Enemy_Attack_ColliderDetector>().ToList();

            foreach (var item in colliderDetector)
            {
                item.playerDetected += ChooseCollider;
            }
        }

        public void HugPlayer(PlayerStats ps)
        {
            // Parenting logic and place onto correct location

            GameObject HugPos = GameObject.Find("HugPos");

            RotateSpeed = 0f;

            EnableNavMesh(false);
            animator.applyRootMotion = true;

            transform.parent = HugPos.transform;

            // animator.enabled = false;

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Collider collider = GetComponent<Collider>();
            collider.enabled = false;

            
        }
    }
}
