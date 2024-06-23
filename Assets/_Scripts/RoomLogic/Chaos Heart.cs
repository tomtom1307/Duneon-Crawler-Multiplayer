using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ChaosHeart : NetworkBehaviour
    {
        public float DamageReduction = 0.5f;
        public DamageableThing DT;
        private void Start()
        {
            DT = GetComponent<DamageableThing>();
        }

        
    }
}
