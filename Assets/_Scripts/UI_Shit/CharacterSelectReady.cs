using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class CharacterSelectReady : NetworkBehaviour
    {
        public static CharacterSelectReady instance { get; private set; }

        private Dictionary<ulong, bool> playerReadyDic;


        private void Awake()
        {
            instance = this;
            playerReadyDic = new Dictionary<ulong, bool>();

        }

        public void SetPlayerReady()
        {
            SetPlayerReadyServerRpc();
        }


        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            playerReadyDic[serverRpcParams.Receive.SenderClientId] = true;

            bool allClientsReady = true;
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if(!playerReadyDic.ContainsKey(clientID)|| !playerReadyDic[clientID]) { 
                    allClientsReady = false;
                    break;
                }
            }

            if (allClientsReady)
            {
                Loader.LoadNetwork(Loader.Scene.TestWorld);
            }
            {
                
            }
        }

    }
}
