using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class RPCTest : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        private void GoofyServerRpc()
        {
            Debug.Log(OwnerClientId);
        }

        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                GoofyServerRpc();
            }
        }
    }
}
