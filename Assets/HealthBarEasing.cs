
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class HealthBarEasing : MonoBehaviour
    {
        // Start is called before the first frame update
        public Image health;
        public Image easing;
        public float Smoothness;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            easing.fillAmount = Mathf.Lerp(easing.fillAmount, health.fillAmount, Smoothness);
        }
    }
}
