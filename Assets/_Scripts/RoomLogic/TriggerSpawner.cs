using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TriggerSpawner : NetworkBehaviour
    {
        public EnemySpawner spawner;
        public GameObject Wall;




        [ServerRpc(RequireOwnership = false)]
        public void OnActivateServerRpc(int Channel)
        {
            
        }


        private void Update()
        {

            
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void TriggerServerRpc()
        {
            print("Triggered");
            //Put Condition for players inside run 
            
            spawner.Active = true;
            
        }


    }

}
