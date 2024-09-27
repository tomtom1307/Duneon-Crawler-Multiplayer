using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ChaosHeartDetection : NetworkBehaviour
    {

        ChaosHeart CH;
        public GameObject Player;
        RoomProgressBar RPB;
        
        private void Start()
        {
            CH = GetComponentInParent<ChaosHeart>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsLocalPlayer) return;
            if(other.gameObject.tag == "Player")
            {
                RPB = other.GetComponentInChildren<RoomProgressBar>();

                RPB.EnableProgressBar(RoomProgressBar.ProgressType.Health, "Chaos Heart", CH.HealthbarColor);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsLocalPlayer) return;
            if (other.gameObject.tag == "Player")
            {
                

                RPB.DisableProgressBar();
                RPB = null;

            }
        }


        private void OnTriggerStay(Collider other)
        {
            
            if(RPB != null)
            {
                RPB.UpdateValue(CH.DT.CurrentHealth.Value / CH.DT.MaxHealth);
            }

            Enemy enemy;
            if (other.TryGetComponent<Enemy>(out enemy))
            {

                if (CH.DT.ded)
                {
                    RPB.DisableProgressBar();
                    enemy.DamageReduction = 1;
                    enemy.RemoveArmorBuff();

                }
                else
                {
                    enemy.AddArmorBuff(CH.BuffVFX);
                    enemy.DamageReduction = CH.DamageReduction;
                }
            }
        }
    }
}
