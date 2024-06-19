using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public abstract class Director : NetworkBehaviour
    {
        [ServerRpc(RequireOwnership = false)]
        public virtual void OnActivateServerRpc(int Channel = 0)
        {
            
        }

        [ServerRpc(RequireOwnership = false)]
        public virtual void OnCompletedServerRpc()
        {
            
        }
    }
}
