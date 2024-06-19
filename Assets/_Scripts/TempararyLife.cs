using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TempararyLife : NetworkBehaviour
    {
        public float LifeTime;
        NetworkObject NO;
        private void Start()
        {
            
            if (TryGetComponent<NetworkObject>(out NO))
            {
                Invoke("DespawnThingyServerRpc", LifeTime);
            }
            
        }


        [ServerRpc(RequireOwnership = false)]
        public void DespawnThingyServerRpc()
        {
            Destroy(gameObject);
            NO.Despawn();
        }


    }
}
