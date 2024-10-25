using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_ColliderDetector : MonoBehaviour
    {
        [HideInInspector] public BoxCollider BoxCollider;
        public event Action<PlayerStats> playerDetected;




        private void Start()
        {
            BoxCollider = GetComponent<BoxCollider>();
            TriggerCollider(false);
        }

        

        public void TriggerCollider(bool X)
        {
            BoxCollider.enabled = X;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerStats player;
                if (other.gameObject.TryGetComponent<PlayerStats>(out player))
                {
                    playerDetected(player);
                }
            }
        }

    }
}
