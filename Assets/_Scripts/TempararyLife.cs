using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TempararyLife : MonoBehaviour
    {
        public float LifeTime;
        NetworkObject NO;
        private void Start()
        {
            
            if (TryGetComponent<NetworkObject>(out NO))
            {
                Invoke("DespawnThingyServerRpc", LifeTime);
            }
            Destroy(gameObject, LifeTime);
        }


        [ServerRpc]
        public void DespawnThingyServerRpc()
        {
            NO.Despawn();
        }


    }
}
