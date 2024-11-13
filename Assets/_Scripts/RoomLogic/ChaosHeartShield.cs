using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaosHeartShield : MonoBehaviour
    {
        public int ActiveTentacles;
        public GameObject Shield;
        public DamageableThing CHDT; 
        public List<ShieldGeneratorTentacle> Tentacles;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void HandleShieldGeneratorTentacle(ShieldGeneratorTentacle SGT, bool active)
        {
            if (!Tentacles.Contains(SGT))
            {
                return;
            }
            if (active)
            {
                ActiveTentacles++;
                if(ActiveTentacles > 0){
                    TriggerShield(true);
                }
            }
            else
            {
                ActiveTentacles--;
                if(ActiveTentacles <= 0) { 
                    TriggerShield(false);
                    

                }
            }
        }

        public void TriggerShield(bool Active)
        {
            Shield.SetActive(Active);
            CHDT.Invincible = Active;
        }


        private void OnEnable()
        {
            Actions.ShieldGeneratorTentacleUpdate += HandleShieldGeneratorTentacle;
        }
    }
}
