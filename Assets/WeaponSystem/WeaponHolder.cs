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
        [field: SerializeField] public WeaponDataSO Data { get; private set; }
        [SerializeField] private float attackCounterResetCooldown = 2f;
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0  : value;
        }

        [SerializeField] List<WeaponComponent> components;
        public event Action OnExit;
        public event Action OnEnter;


        public Animator anim;
        public GameObject baseGO { get; private set; }
        public AnimationEventHandler eventHandler { get; private set; }
        private int currentAttackCounter;
        public VFXHandler visualAttacks;
        private Timer attackCounterResetTimer;



        private void Awake()
        {
            components = GetComponents<WeaponComponent>().ToList();
            
            baseGO = transform.Find("Base").gameObject;
            eventHandler = baseGO.GetComponent<AnimationEventHandler>();
            anim = GetComponentInChildren<Animator>();
            
            attackCounterResetTimer = new Timer(attackCounterResetCooldown);



        }


        private void Update()
        {
            attackCounterResetTimer.Tick();
            
        }

        private void ResetAttackCounter() => CurrentAttackCounter = 0;


        public void Enter()
        {
            visualAttacks = GetComponentInChildren<VFXHandler>();
            anim.SetBool("active", true);
            anim.SetInteger("counter", CurrentAttackCounter);
            attackCounterResetTimer.StopTimer();

            OnEnter?.Invoke();
        }

        public void EnterSecondaryAttack()
        {
            anim.SetBool("Secondary", true);
            anim.SetBool("active", true);
        }


        public void Exit()
        {
            anim.SetBool("active", false);
            anim.SetBool("Secondary", false);
            anim.SetBool("SecondaryRelease", false);
            CurrentAttackCounter++;
            attackCounterResetTimer.StartTimer();
            OnExit?.Invoke();
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
            attackCounterResetTimer.OntimerDone -= ResetAttackCounter;
            eventHandler.OnFinish -= Exit;
        }
    }
}

