using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class DisplayStatsUI : MonoBehaviour
    {
        public static DisplayStatsUI Singleton;
        public TextMeshProUGUI LevelVal;
        public TextMeshProUGUI MaxHealthVal;
        public TextMeshProUGUI MaxManaVal;
        public TextMeshProUGUI StrengthVal;
        public TextMeshProUGUI IntelligenceVal;
        public TextMeshProUGUI DexVal;
        public Image XPbar;
        public TextMeshProUGUI XPText;
        public TextMeshProUGUI GoldText;
        public TextMeshProUGUI SkillPointsText;
        public TextMeshProUGUI WhiteCrystal;
        public TextMeshProUGUI GreenCrystal;
        public TextMeshProUGUI BlueCrystal;
        public TextMeshProUGUI PurpleCrystal;
        public TextMeshProUGUI OrangeCrystal;


        private void Start()
        {
            Singleton = this;
            
        }

        public void UpdateXPBar(int xp, int requiredxp)
        {
            float totalxp = (float)(requiredxp + xp);
            XPbar.fillAmount = ((float)xp / (totalxp));
            XPText.text = new string($"{xp}/{totalxp}");
        }

        public void UpdateUIStat(int wantedVal, TextMeshProUGUI text)
        {
            text.text = wantedVal.ToString();
        }

        public void ChangeGoldValue(int val)
        {
            UpdateUIStat(val, GoldText);
        }

        public void ChangeSkillPoints(int val)
        {
            UpdateUIStat(val, SkillPointsText);
        }

    }
}
