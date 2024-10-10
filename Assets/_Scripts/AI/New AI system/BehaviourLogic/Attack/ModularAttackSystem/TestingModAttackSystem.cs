using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Project.Weapons.WeaponHolder;

namespace Project
{
    public class TestingModAttackSystem : MonoBehaviour
    {

        [HideInInspector] public Enemy_Attack_ColliderDetector ColliderDetector;

        Enemy_Attack AttackInstance;


        public Animator anim;


        public EnemyStateMachine StateMachine { get; set; }
        public EnemyAttackState AttackState { get; set; }
        public EnemyAttackSOBase EnemyAttackInstance { get; set; }
        [SerializeField] private EnemyAttackSOBase EnemyAttackBase;




        public virtual void InitializeStateMachine()
        {
            //Initialize StateMachine
            //EnemyAttackInstance.Initialize(gameObject, this);

            StateMachine.Initialize(AttackState);
        }

        private void Awake()
        {
            //Contruct StateMachine
            StateMachine = new EnemyStateMachine();

            //Contruct States
            //AttackState = new EnemyAttackState(this, StateMachine);
        }


        private void Start()
        {
            ColliderDetector = GetComponentInChildren<Enemy_Attack_ColliderDetector>();
            ColliderDetector.playerDetected += DoDamage;


            EnemyAttackInstance = Instantiate(EnemyAttackBase);

            InitializeStateMachine();
        }


        public void Update()
        {
            StateMachine.currentState.FrameUpdate();
        }


        public void DoDamage(PlayerStats player)
        {
            player.TakeDamage(2, transform.position);
        }

        private void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            StateMachine.currentState.AnimationTriggerEvent(triggerType);
        }


        
    }
}
