using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class EnterDungeonPortal : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            EnterDungeonServerRpc();
        }


        [ServerRpc(RequireOwnership =false)]
        public void EnterDungeonServerRpc()
        {
            Loader.LoadNetwork(Loader.Scene.ProceduralTestingScene);
        }
    }
}
