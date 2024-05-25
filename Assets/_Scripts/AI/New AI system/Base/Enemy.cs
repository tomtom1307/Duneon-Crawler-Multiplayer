using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    public class Enemy : NetworkBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set ; }
        [field: SerializeField] public float maxDetectDist { get; set; } = 100f;
        [field: SerializeField] public float AttackDistance { get; set; } = 4f;
        [field: SerializeField] public float lungeDistance { get; set; } = 4f;
        [field: SerializeField] public float TimeBetweenAttacks { get; set; } = 4f;
        public Rigidbody rb { get; set ; }
        public NavMeshAgent navMesh { get; set; }

        public Animator animator { get; set; }

        public List<Transform> playerlist;
        public Transform target;
        public float aggression = 0.5f;
        


        public EnemyStateMachine StateMachine { get; set; }
        public EnemyIdleState IdleState { get; set; }
        public EnemyChaseState ChaseState { get; set; }
        public EnemyAttackState AttackState { get; set; }
        public bool IsWithinStrikingDistance { get; set; }

        public float RandomMovementRange = 5f;
        public float RandomMovementSpeed = 1f;



        private void Awake()
        {
            //Contruct StateMachine
            StateMachine = new EnemyStateMachine();


            //Contruct States
            IdleState = new EnemyIdleState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
        }

        private void Start()
        {
            //Set MaxHealth
            CurrentHealth = MaxHealth;
            //Get References
            rb = GetComponent<Rigidbody>(); 
            navMesh = GetComponent<NavMeshAgent>();
            
            animator = GetComponent<Animator>();

            //Find Players in lobby
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }

            //Initialize StateMachine
            StateMachine.Initialize(ChaseState);

            //Final Setup shit
            rb.isKinematic = true;
        }


        #region Health Stuff

        public void Damage(float DamageAmount)
        {
            CurrentHealth = DamageAmount;

            if(CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            
        }

        #endregion

        public void MoveEnemy(Vector3 targetPosition)
        {
            navMesh.SetDestination(targetPosition);
        }

        public virtual void AttackExit()
        {
            animator.SetBool("Attacking", false);
        }


        public void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }



        private void Update()
        {
            
            animator.SetFloat("SpeedX", Vector3.Dot(navMesh.velocity,transform.right));
            animator.SetFloat("SpeedY", Vector3.Dot(navMesh.velocity, transform.forward));
            StateMachine.currentState.FrameUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.currentState.PhysicsUpdate();
        }

        public Transform DetectPlayer()
        {
            float MinDist = maxDetectDist;
            Transform target = null;
            foreach (var item in playerlist)
            {
                if (Vector3.Distance(transform.position, item.position) < MinDist)
                {
                    MinDist = Vector3.Distance(transform.position, item.position);
                    target = item;
                }
            }


            return target;
        }



        #region Animation Triggers

        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.currentState.AnimationTriggerEvent(triggerType);
        }

        public void SetStrikingDistanceBool(bool isStriking)
        {
            IsWithinStrikingDistance = isStriking;
        }

        public enum AnimationTriggerType
        {
            EnemyDamaged,
            PlayFootStepSound
        }


        #endregion

    }
}
