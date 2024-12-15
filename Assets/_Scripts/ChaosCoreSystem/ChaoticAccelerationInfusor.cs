using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaoticAccelerationInfusor : _Interactable
    {
        private PlayerStats PS;
        private StatManager WS;
        string OGPrompt;
        public float SpeedPercentageBonus = 1f;
        public int UpgradePrice = 5;
        private void Start()
        {
            OGPrompt = Prompt;
            Prompt = $"{UpgradePrice}" + OGPrompt;
        }
        protected override void Interact()
        {

            PS = interacter.GetComponent<PlayerStats>();
            if (PS.ChaosCores >= UpgradePrice)
            {
                PS.ChaosAgility += SpeedPercentageBonus;
                
                PS.ChaosCores -= UpgradePrice;
                PS.ChaosCoreCountUI.text = $"{PS.ChaosCores}";
                UpgradePrice += 2;
                Prompt = $"{UpgradePrice}" + OGPrompt;
            }


        }
    }
}
