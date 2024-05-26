using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    public class EnemyChaseState : EnemyState
    {
        private Vector3 _targetPos;
        public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            enemy.target = enemy.DetectPlayer();
            enemy.animator.SetBool("Attacking", false);
            _targetPos = enemy.target.position;
            enemy.navMesh.stoppingDistance = enemy.AttackDistance*0.8f;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            //If Distance is less than attack value retreat or attack depending on state
            base.FrameUpdate();
            _targetPos = enemy.target.position;
            enemy.MoveEnemy(_targetPos);
            if (enemy.IsWithinStrikingDistance)
            {
                enemy.StateMachine.ChangeState(enemy.AttackState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
