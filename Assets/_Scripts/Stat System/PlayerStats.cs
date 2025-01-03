using Project.Assets._Scripts.Stat_System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Project
{
    public class PlayerStats : NetworkBehaviour
    {
        public enum CrystalType
        {
            white,
            green,
            blue,
            purple,
            orange
        }

    
        [Header("Levels")]
        public int xp;
        public int requiredXP;
        public int level = 1;
        public AnimationCurve LevelXPScaling;

        [Header("Stats")]
        public float MaxHealth;
        public float MaxMana;
        public float Armor;
        public float Agility;
        public float Strength;
        public float Intelligence;
        public float Dexterity;

        [Header("Chaos Stats")]
        public float ChaosHealth;
        public float ChaosMana;
        public float ChaosAgility;
        public float ChaosStrength;
        public float ChaosIntelligence;
        public float ChaosDexterity;


        [Header("Currency")]
        public int ChaosCores;
        public int Gold;
        public int SkillPoints;

        [Header("Upgrade Crystals")]
        private Dictionary<CrystalType, int> crystalCounts = new Dictionary<CrystalType, int>();

        [Header("Damage Visuals")]
        public CamShake CS;
        public DamageArrowUI DamageArrow;

        [Header("Health and UI")]
        [SerializeField] public NetworkVariable<float> _health = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        [SerializeField] public NetworkVariable<float> _mana = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        public Image healthBarFill;
        public Image manaBarFill;
        public TextMeshProUGUI ChaosCoreCountUI;
        public float manaRegen;
        public StatManager StatManager;

        

        private void Awake()
        {
            
        }


        //Initialization
        private void Start()
        {
            if (!IsLocalPlayer)
            {
                return;
            }
            //Find StatManager
            StatManager = FindAnyObjectByType<StatManager>();
            print(StatManager);
            StatManager.stats = this;
            StatManager.InitDamageVals();

            //Initialize health
            _health.Value = MaxHealth;
            healthBarFill.fillAmount = 1;
            _mana.Value = MaxMana;
            manaBarFill.fillAmount = 1;
            //Initialize level
            requiredXP = GetRequiredXP(level);

            //Init UI display
            DisplayStatsUI.Singleton.UpdateUIStat(level, DisplayStatsUI.Singleton.LevelVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)MaxHealth, DisplayStatsUI.Singleton.MaxHealthVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)MaxMana, DisplayStatsUI.Singleton.MaxManaVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Strength, DisplayStatsUI.Singleton.StrengthVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Intelligence, DisplayStatsUI.Singleton.IntelligenceVal);
            DisplayStatsUI.Singleton.UpdateUIStat((int)Dexterity, DisplayStatsUI.Singleton.DexVal);
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);
            ChaosCoreCountUI.text = $"{ChaosCores}";

            //Get CamShake Component
            CS = Camera.main.GetComponent<CamShake>();
            //DamageArrow component

            // Initialize the crystal counts (all starting from 0)
            foreach (CrystalType type in System.Enum.GetValues(typeof(CrystalType)))
            {
                crystalCounts[type] = 0;
            }

        }

        [ClientRpc]
        public void AddManaClientRpc(float Amount)
        {
            if(!IsLocalPlayer) return;
            _mana.Value += Amount;
        }


        private void Update()
        {
            
            RegenManaClientRpc();
        }

        [ClientRpc]
        public void RegenManaClientRpc()
        {
            if (!IsLocalPlayer) return;
            _mana.Value += manaRegen * Time.deltaTime;
            _mana.Value = Mathf.Clamp(_mana.Value, 0, MaxMana);
            manaBarFill.fillAmount = _mana.Value / MaxMana;
        }


        //Handle Damage stuff
        public bool DoMagicAttack(float manaUse,bool ChangeMana = true)
        {
            if (manaUse > _mana.Value)
            {
                //Handle Feedback
                return false;
            }
            if (ChangeMana)
            {
                _mana.Value -= manaUse;
                manaBarFill.fillAmount = _mana.Value / MaxHealth;
            }
            
            return true;
        }




        [ContextMenu("Test Shit")]
        public void RefreshStats()
        {
            StatManager.InitDamageVals();
        }


        //Handle Health



        public void TakeDamage(float damage, Vector3 damageOrigin, SourceSoundManager.SoundType soundType = SourceSoundManager.SoundType.Hit)
        {   
            if(IsOwner)
            {
                CS.StartShake(CS.onHit);
                if(damageOrigin != Vector3.zero) { HandleDamageArrowShit(damageOrigin); }
                
            }
            
            
            if (!IsLocalPlayer) return;
            damage -= Armor;
            damage = Mathf.Clamp(damage, 0, MaxHealth - 1);

            
            _health.Value -= damage;
            healthBarFill.fillAmount = _health.Value / MaxHealth;
            PlayerSoundSource.Instance.PlaySound(soundType, 0.7f);
            //HealthBar Animations
            //DamageSounds
            //Camera effects

            if (_health.Value <= 0)
            {
                //Enter Reiviable state
                // in revivable state to fight for you life Create a bar that has a marker that bounces left and right and 
                // if the input is given at the right time the revive timer increases slightly to buy time for teammates to revive you
            }
        }


        public void HandleDamageArrowShit(Vector3 damageOrigin)
        {
            DamageArrow.DamageLocation = damageOrigin;
            GameObject GO = Instantiate(DamageArrow.gameObject, DamageArrow.transform.position, DamageArrow.transform.rotation, DamageArrow.transform.parent);
            GO.SetActive(true);
            Destroy(GO, 6);
        }


        //Handle Levels and XP


        public int GetRequiredXP(int level)
        {
            
            int requiredXP = (int)LevelXPScaling.Evaluate(level);
            return requiredXP;
        }

        public void LevelUp()
        {
            level++;
            SkillPoints++;
            DisplayStatsUI.Singleton.UpdateUIStat(level, DisplayStatsUI.Singleton.LevelVal);
            DisplayStatsUI.Singleton.ChangeSkillPoints(SkillPoints);
            MaxHealth += 10;
            MaxMana += 10;
            manaRegen += 0.2f;
            requiredXP = GetRequiredXP(level);
            xp = 0;
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);
        }

        #region CrystalHandling
        public void SetCrystalCount(CrystalType type, int count)
        {
            if (crystalCounts.ContainsKey(type))
            {
                crystalCounts[type] = count;
                
                Debug.Log($"Set {type} crystal count to {count}");
            }
            UpdateCrystalUI();
        }

        // Method to increment the number of crystals for a specific type
        public void AddCrystals(CrystalType type, int amount)
        {
            if (crystalCounts.ContainsKey(type))
            {
                crystalCounts[type] += amount;
                Debug.Log($"Added {amount} {type} crystals. New count: {crystalCounts[type]}");
            }
            UpdateCrystalUI();
        }

        public int GetCrystalCount(CrystalType type)
        {
            if (crystalCounts.ContainsKey(type))
            {
                return crystalCounts[type];
            }

            return 0;
        }


        private void UpdateCrystalUI()
        {
            DisplayStatsUI.Singleton.WhiteCrystal.text = GetCrystalCount(CrystalType.white).ToString();
            DisplayStatsUI.Singleton.GreenCrystal.text = GetCrystalCount(CrystalType.green).ToString();
            DisplayStatsUI.Singleton.BlueCrystal.text = GetCrystalCount(CrystalType.blue).ToString();
            DisplayStatsUI.Singleton.PurpleCrystal.text = GetCrystalCount(CrystalType.purple).ToString();
            DisplayStatsUI.Singleton.OrangeCrystal.text = GetCrystalCount(CrystalType.orange).ToString();
         
        }

        #endregion


        public void AddXp(int xpToAdd)
        {
            print("XP added!!!!");
            if(!IsLocalPlayer) return;
            if(xpToAdd >= requiredXP)
            {
                xpToAdd -= requiredXP;
                LevelUp();
            }
            requiredXP -= xpToAdd;
            xp += xpToAdd;
            DisplayStatsUI.Singleton.UpdateXPBar(xp, requiredXP);
        }

        [ClientRpc]
        public void AddXpClientRpc(int xpToAdd)
        {
            AddXp(xpToAdd);
        }


        [ClientRpc]
        public void AddGoldClientRpc(int goldToAdd)
        {
            Gold += goldToAdd;
            DisplayStatsUI.Singleton.ChangeGoldValue(Gold);
        }





    }
}
