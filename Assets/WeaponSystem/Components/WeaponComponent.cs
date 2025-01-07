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


        public enum AttackType
        {
            Attack1,
            Attack2
        }

        public AttackType attackType;

        public virtual void Init()
        {

        }

        protected virtual void Start()
        {
            weapon = GetComponent<WeaponHolder>();
            eventHandler = GetComponentInChildren<AnimationEventHandler>();
            weapon.OnEnter += HandleEnter;

        }





        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }


        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }



        protected virtual void OnDestroy()
        {
            weapon.OnEnter -= HandleEnter;
        }



    }


    public abstract class WeaponComponent<T1,T2> : WeaponComponent where T1: ComponentData<T2> where T2: AttackData
    {
        protected T1 data1;
        protected T1 data2;
        protected T1 data;
        protected T2 currentAttackData;


        protected override void HandleEnter()
        {
            base.HandleEnter();
             
            currentAttackData = data.AttackData[weapon.CurrentAttackCounter];

        }

        protected override void Start()
        {
            base.Start();

            data = weapon.Data.GetData<T1>();
        }
    }
    
}
