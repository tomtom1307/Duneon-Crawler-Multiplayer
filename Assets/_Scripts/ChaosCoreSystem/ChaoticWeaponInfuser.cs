using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaoticWeaponInfuser : _Interactable
    {
        private PlayerStats PS;
        private StatManager WS;
        string OGPrompt;
        public float DamageUpgrade;
        public int UpgradePrice = 5;
        private void Start()
        {
            OGPrompt = Prompt;
            Prompt = $"{UpgradePrice}"+OGPrompt;
        }
        protected override void Interact()
        {
            
            PS = interacter.GetComponent<PlayerStats>();
            WS = interacter.GetComponent<HandleCam>().PC.gameObject.GetComponentInChildren<StatManager>();
            if(PS.ChaosCores >= UpgradePrice)
            {
                WS.weaponInstance.ChaosBonus += DamageUpgrade;
                
                WS.ChaosBonus = WS.weaponInstance.ChaosBonus;
                PS.ChaosCores -= UpgradePrice;
                PS.ChaosCoreCountUI.text = $"{PS.ChaosCores}";
                UpgradePrice += 2;
                Prompt = $"{UpgradePrice}" + OGPrompt;
            }


        }
    }
}
