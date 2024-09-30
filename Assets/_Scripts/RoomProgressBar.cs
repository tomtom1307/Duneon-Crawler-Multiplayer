using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class RoomProgressBar : NetworkBehaviour
    {
        public Image Fill;
        public TextMeshProUGUI Text;
        public GameObject GO;
        HealthBarEasing HBS;
        public ProgressType PT;

        private void Start()
        {
            HBS = GetComponent<HealthBarEasing>();
            DisableProgressBar();
        }


        public enum ProgressType
        {
            Health,
            Charge
        }


        public void EnableProgressBar(ProgressType T, string label, Color color)
        {
           
            Fill.color = color;
            Text.text = label;

            if(T == ProgressType.Health)
            {
                Fill.fillAmount = 1;
                HBS.ResetEasing();

            }
            else
            {
                Fill.fillAmount = 0;
            }

            GO.SetActive(true);
        }

        public void DisableProgressBar()
        {
            GO.SetActive(false);
        }

        public void UpdateValue(float val)
        {
            Fill.fillAmount = val;
        }
    }
}
