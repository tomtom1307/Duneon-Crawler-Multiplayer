using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CrossBowTurret : Enemy
    {
        public Transform Barrel;
        public float AimRotation;
        public Vector3 initDir;
        public override void InitializeStateMachine()
        {
            //Initialize StateMachine
            EnemyChaseInstance.Initialize(gameObject, this);
            EnemyAttackInstance.Initialize(gameObject, this);

            StateMachine.Initialize(AttackState);
            initDir = Barrel.forward;
            
        }

        public void RotateTowardsPlayer()
        {
            if (Vector3.Dot(initDir, target.position - transform.position) < 0.7f)
            {
                target = null;
                return;
            }
            // Get the desired rotation
            Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);

          
            // Apply the clamped rotation
            Barrel.rotation = Quaternion.Slerp(Barrel.rotation, desiredRotation, 0.1f);


        }

        public override void Update()
        {
            StateMachine.currentState.FrameUpdate();
            if (target == null)
            {
                
                target = DetectPlayer();
                if(Vector3.Dot(initDir, target.position - transform.position) < 0.6f )
                {
                    target = null;
                    StateMachine.ChangeState(ChaseState);
                }
                IsWithinStrikingDistance = true;
                StateMachine.ChangeState(AttackState);
            }
            else
            {
                RotateTowardsPlayer();
            }
        }
    }
}
