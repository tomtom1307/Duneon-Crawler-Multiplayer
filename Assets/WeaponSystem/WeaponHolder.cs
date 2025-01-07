using Project.Assets.WeaponSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static UnityEditor.Progress;


namespace Project.Weapons
{
    public class WeaponHolder : NetworkBehaviour
    {

        [SerializeField] private float attackCounterResetCooldown = 2f;
        public StatManager statManager { get; private set; }
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value == Data.NumberOfAttacks ? 0 : value;
        }

        public int Counter;

        [SerializeField] public WeaponDataSO Data;
        WeaponGenerator WG;
        public WeaponInstance[] WeaponDatas;
        public int currentWeaponIndex;
        public float SwapCoolDown;
        public bool Swapping;
        public int HeldWeaponAmount;

        [SerializeField] List<WeaponComponent> components;
        public event Action OnExit;
        public event Action OnEnter;
        public float VelocityReduction;

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


        public enum AttackType
        {
            attack1,
            attack2
        }

        public AttackType attackType = AttackType.attack1;


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
            WG = GetComponent<WeaponGenerator>();
            _soundSource = GetComponentInChildren<PlayerSoundSource>();
            WeaponDatas = new WeaponInstance[HeldWeaponAmount];
            allSlotsEmpty = true;

        }

        public void SetData(WeaponDataSO data)
        {
            Data = data;
        }


        private void Update()
        {
            attackCounterResetTimer.Tick();
            Counter = currentAttackCounter;

            if (allSlotsEmpty || State == AttackState.active) return;

            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (scroll > 0) // Scroll Up
            {
                NextWeaponSlot();
            }
            else if (scroll < 0) // Scroll Down
            {
                PreviousWeaponSlot();
            }

        }

        public void NextWeaponSlot()
        {
            if (allSlotsEmpty) return;

            int startingIndex = currentWeaponIndex; // Store the original index
            int attempts = 0;

            do
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % HeldWeaponAmount;
                attempts++;

                if (WeaponDatas[currentWeaponIndex] != null && WeaponDatas[currentWeaponIndex].weaponData != null)
                {
                    SwitchWeapon(currentWeaponIndex);
                    return;
                }

            } while (attempts < HeldWeaponAmount);

            // Revert to the original index if no valid weapon is found
            currentWeaponIndex = startingIndex;
            Debug.Log("No valid weapons found when scrolling up.");
        }





        public void PreviousWeaponSlot()
        {
            if (allSlotsEmpty) return;

            int startingIndex = currentWeaponIndex;
            int attempts = 0;

            do
            {
                currentWeaponIndex--;
                if (currentWeaponIndex < 0)
                    currentWeaponIndex = HeldWeaponAmount - 1;

                attempts++;

                // Break the loop if we've gone through all slots
                if (attempts >= HeldWeaponAmount)
                {
                    Debug.Log("No valid weapons found when scrolling down.");
                    return;
                }

            } while (WeaponDatas[currentWeaponIndex] == null);

            // Switch to the valid weapon found
            SwitchWeapon(currentWeaponIndex);
        }





        public void SwitchWeapon(int index)
        {
            currentAttackCounter = 0;

            if (WeaponDatas[index] == null || WeaponDatas[index].weaponData == null)
            {
                Debug.Log("Invalid weapon slot. Swap Aborted.");
                return;
            }

            if (WeaponDatas[index] == statManager.weaponInstance)
            {
                Debug.Log("Already using this weapon. Swap Aborted.");
                return;
            }

            WG.SwapWeapon(WeaponDatas[index]);
            statManager.weaponInstance = WeaponDatas[index];
        }


        public void SlotWeapon(WeaponInstance WI, int SlotVal)
        {
            Debug.Log("Slotting Weapon");
            WeaponDatas[SlotVal] = WI;

            UpdateInventoryState();

            if (currentWeaponIndex == SlotVal || allSlotsEmpty)
            {
                currentWeaponIndex = SlotVal;
                SwitchWeapon(currentWeaponIndex);
            }
        }


        public void RemoveWeapon(int SlotVal)
        {
            if (WeaponDatas[SlotVal] != null)
            {
                Debug.Log($"Removing weapon from slot {SlotVal}");
                WeaponDatas[SlotVal] = null;

                if (SlotVal == currentWeaponIndex)
                {
                    WG.RemoveWeapon();
                    statManager.weaponInstance = null;
                    Data = null;

                    // Attempt to switch to the next available weapon
                    NextWeaponSlot();
                }

                UpdateInventoryState();
            }
        }



        public bool allSlotsEmpty;

        void UpdateInventoryState()
        {
            allSlotsEmpty = AreAllSlotsEmpty();
            if (allSlotsEmpty)
            {
                Debug.Log("All weapon slots are empty!");
                currentWeaponIndex = 0; // Reset to a default index
            }
        }


        bool AreAllSlotsEmpty()
        {
            foreach (WeaponInstance weapon in WeaponDatas)
            {
                Debug.Log(weapon);
                if (weapon != null)
                    return false; // If any slot is not empty, return false
            }
            return true; // All slots are empty
        }


        private void ResetAttackCounter() => CurrentAttackCounter = 0;




        public void Enter(int attack)
        {
            anim.SetFloat("AttackSpeed", statManager.AttackSpeed);
            if(State == AttackState.active|| State == AttackState.coolDown )
            {
                return;
            }

            switch (attack)
            {
                
                case (1):
                {
                        attackType = AttackType.attack1;

                        if(Data.Attack1Type == DamageType.Magic)
                        {
                            if (!statManager.stats.DoMagicAttack(Data.Attack1ManaUse))
                            {
                                return;
                            }
                        }
                        Cooldown = Data.Attack1Cooldown;
                        State = AttackState.active;
                        
                        anim.SetBool("active", true);
                        anim.SetInteger("counter", CurrentAttackCounter);
                        attackCounterResetTimer.StopTimer();

                        OnEnter?.Invoke();
                        break;
                }

                case (2):
                {

                    attackType = AttackType.attack2;

                    Cooldown = Data.Attack2Cooldown;
                    
                    State = AttackState.active;
                    anim.SetBool("Secondary", true);
                    anim.SetBool("active", true);

                    OnEnter?.Invoke();
                    break;
                }
                

            }

            
        }

        public void DoAttackScreenShake()
        {
            CamShake CS = Camera.main.GetComponent<CamShake>();
            CS.StartShake(CS.onAttack);
        }

        public void DoAOEScreenShake()
        {
            CamShake CS = Camera.main.GetComponent<CamShake>();
            CS.StartShake(CS.onAOE);
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
            eventHandler.OnAttackAction += DoAttackScreenShake;
            eventHandler.OnAOEAction += DoAOEScreenShake;
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
            eventHandler.OnAttackAction -= DoAttackScreenShake;
            eventHandler.OnAOEAction -= DoAOEScreenShake;
            eventHandler.OnFinish -= Exit;
        }
    }
}

