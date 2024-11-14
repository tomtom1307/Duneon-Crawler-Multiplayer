
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class HealthBarEasing : NetworkBehaviour
    {
        // Start is called before the first frame update
        public Image health;
        public Image easing;
        public float Smoothness;
        public float DelayTime = 0.25f;

        float timer;

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

            if(health.fillAmount != easing.fillAmount && timer < DelayTime)
            {
                timer += Time.deltaTime;
                return;
            }
            
            easing.fillAmount = Mathf.Lerp(easing.fillAmount, health.fillAmount, Smoothness);

            if(easing.fillAmount - health.fillAmount < 0.001f)
            {
                timer = 0;
            }
        }


        public void ResetEasing()
        {
            easing.fillAmount = 1;
        }
    }
}
