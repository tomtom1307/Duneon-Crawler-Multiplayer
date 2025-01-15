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

        public bool InUse;
       
        public List<int> Attacks = new List<int>();

        public virtual void Init()
        {
            
        }

        protected virtual void Start()
        {
            weapon = GetComponent<WeaponHolder>();

            if (weapon == null)
            {
                Debug.LogError("WeaponHolder component is missing on this GameObject!");
                return;
            }

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

        public IEnumerator WaitForDataInitialization()
        {
            if(weapon == null)
            {
                Debug.Log("No WeaponHolder Found getting component now");
                weapon = GetComponent<WeaponHolder>();
            }
            while (weapon.Data == null)
            {
                Debug.Log("Waiting for weapon.Data to be initialized...");
                yield return null; // Wait for the next frame
            }

            Debug.Log("weapon.Data is initialized. Proceeding with setup.");
            Init();
        }

    }


    public abstract class WeaponComponent<T1,T2> : WeaponComponent where T1: ComponentData<T2> where T2: AttackData
    {
        [SerializeField] protected T1 data1;
        [SerializeField] protected T1 data2;
        protected T1 data;
        protected T2 currentAttackData;
        

        protected override void HandleEnter()
        {
            
            Debug.Log("WeaponComponent:" + typeof(T1).Name + ":" + InUse);
            if (!InUse) return;
            base.HandleEnter();
            if (weapon.attackType == WeaponHolder.AttackType.attack1)
            {
                if (!Attacks.Contains(1)) return;
                data = data1;
            }
            else if (weapon.attackType == WeaponHolder.AttackType.attack2)
            {
                if (!Attacks.Contains(2)) return;
                data = data2;

            }
            else if (data == null)
            {
                Debug.LogError("Found No Datas!");
                return;
            }
            else if(data.AttackData == null)
            {
                Debug.LogError("Found No Attack Data List");
            }
            else if (data.AttackData[weapon.CurrentAttackCounter] == null)
            {
                Debug.LogError("Found No Attack Data");
            }
            currentAttackData = data.AttackData[weapon.CurrentAttackCounter];
            

        }

        protected override void Start()
        {
            base.Start();

            StartCoroutine(WaitForDataInitialization());

        }

        public override void Init()
        {
            Attacks.Clear(); 
            Debug.Log("Weapon Data is"+ weapon.Data);

            //data = weapon.Data.GetData<T1>();
            data1 = weapon.Data.GetData<T1>(1);
            if(data1 == null)
            {
                Debug.Log(this.name + "Data1 is null");
                
            }
            else
            {
                Attacks.Add(1);
                print(data1);
            }
        
            data2 = weapon.Data.GetData<T1>(2);

            if (data2 == null)
            {
                Debug.Log("WeaponComponent:" + typeof(T1).Name + "Data2 is null");
            }
            else
            {
                print(data2);
                Attacks.Add(2);
            }
            
        }

        
    }
    
}
