using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaosHeartDetection : MonoBehaviour
    {

        ChaosHeart CH;

        private void Start()
        {
            CH = GetComponentInParent<ChaosHeart>();
        }

        private void OnTriggerStay(Collider other)
        {
            Enemy enemy;
            if (other.TryGetComponent<Enemy>(out enemy))
            {
                if (CH.DT.ded)
                {
                    enemy.DamageReduction = 1;
                }
                else
                {
                    enemy.DamageReduction = CH.DamageReduction;
                }
            }
        }
    }
}
