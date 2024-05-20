using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


namespace Project.Weapons
{
    public class WeaponHolder : NetworkBehaviour
    {
        
        [SerializeField] private float attackCounterResetCooldown = 2f;
        public StatManager statManager { get; private set; }
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0  : value;
        }

        [SerializeField] public WeaponDataSO Data;
        [SerializeField] List<WeaponComponent> components;
        public event Action OnExit;
        public event Action OnEnter;


        [HideInInspector] public Animator anim;
        [HideInInspector] public float Cooldown = 0.2f;
        [HideInInspector] public GameObject baseGO { get; private set; }
        public AnimationEventHandler eventHandler { get; private set; }
        private int currentAttackCounter;
        public VFXHandler visualAttacks;
        private Timer attackCounterResetTimer;
        private int CurrentAttack;
        public PlayerSoundSource _soundSource;
        public enum AttackState
        {
            ready,
            active,
            coolDown
        }
        [SerializeField] public AttackState State = AttackState.ready;


        private void Awake()
        {
            //components = GetComponents<WeaponComponent>().ToList();
            statManager = GetComponent<StatManager>();
            baseGO = transform.Find("Base").gameObject;
            eventHandler = baseGO.GetComponent<AnimationEventHandler>();
            anim = GetComponentInChildren<Animator>();
            
            attackCounterResetTimer = new Timer(attackCounterResetCooldown);



        }

        private void Start()
        {
            _soundSource = GetComponentInChildren<PlayerSoundSource>();
        }

        public void SetData(WeaponDataSO data)
        {
            Data = data;
            visualAttacks = GetComponentInChildren<VFXHandler>();
        }


        private void Update()
        {
            attackCounterResetTimer.Tick();
            
        }

        private void ResetAttackCounter() => CurrentAttackCounter = 0;




        public void Enter(int attack)
        {

            if(State == AttackState.active|| State == AttackState.coolDown )
            {
                return;
            }

            switch (attack)
            {
                
                case (1):
                {
                        if(Data.Attack1Type == DamageType.Magic)
                        {
                            if (!statManager.stats.DoMagicAttack(Data.Attack1ManaUse))
                            {
                                return;
                            }
                        }
                        Cooldown = Data.Attack1Cooldown;
                        State = AttackState.active;
                        visualAttacks = GetComponentInChildren<VFXHandler>();
                        anim.SetBool("active", true);
                        anim.SetInteger("counter", CurrentAttackCounter);
                        attackCounterResetTimer.StopTimer();

                        OnEnter?.Invoke();
                        break;
                }

                case (2):
                {
                        
    
                        visualAttacks = GetComponentInChildren<VFXHandler>();
                    Cooldown = Data.Attack2Cooldown;
                    State = AttackState.active;
                    anim.SetBool("Secondary", true);
                    anim.SetBool("active", true);

                    OnEnter?.Invoke();
                    break;
                }
                

            }

            
        }



        public void Exit()
        {
            StartCoroutine(cooldown(Cooldown));
            anim.SetBool("active", false);
            anim.SetBool("Secondary", false);
            CurrentAttackCounter++;
            attackCounterResetTimer.StartTimer();

            OnExit?.Invoke();
        }

        IEnumerator cooldown(float cooldown)
        {
            State = AttackState.coolDown;
            yield return new WaitForSeconds(cooldown);
            State = AttackState.ready;
            yield return null;
        }

        private void OnEnable()
        {
            anim.enabled = true;
            foreach (var item in components)
            {
                item.enabled = true;
            }
            eventHandler.OnFinish += Exit;
            attackCounterResetTimer.OntimerDone += ResetAttackCounter;
        }

        private void OnDisable()
        {
            anim.enabled = false;
            foreach (var item in components)
            {
                item.enabled = false;
            }
            anim.SetBool("active", false);
            anim.SetBool("Secondary", false);
            anim.SetBool("SecondaryRelease", false);
            State = AttackState.ready;
            attackCounterResetTimer.OntimerDone -= ResetAttackCounter;
            eventHandler.OnFinish -= Exit;
        }
    }
}

