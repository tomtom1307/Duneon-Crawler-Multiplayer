using Project.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public abstract class WeaponComponent : NetworkBehaviour
    {
        protected WeaponHolder weapon;

        protected bool isAttackActive;
        //TODO: Fix when finsihing weapon Data
        //protected AnimationEventHandler eventHandler => weapon.EventHandler;
        protected AnimationEventHandler eventHandler;

        protected virtual void Awake()
        {
            weapon = GetComponent<WeaponHolder>();
            eventHandler = GetComponentInChildren<AnimationEventHandler>();
            
        }

        protected virtual void Start()
        {
            
        }



        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }


        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }


        protected virtual void OnEnable()
        {
            
            weapon.OnEnter += HandleEnter;
        }

        protected virtual void OnDisable()
        {
            weapon.OnEnter -= HandleEnter;
        }



    }


    public abstract class WeaponComponent<T1,T2> : WeaponComponent where T1: ComponentData<T2> where T2: AttackData
    {
        protected T1 data;
        protected T2 currentAttackData;


        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentAttackData = data.AttackData[weapon.CurrentAttackCounter];

        }

        protected override void Awake()
        {
            base.Awake();

            data = weapon.Data.GetData<T1>();
        }
    }
    
}
