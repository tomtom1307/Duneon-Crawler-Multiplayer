using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class GameManager : NetworkBehaviour
    {

        [SerializeField] private Transform PlayerPrefab;

        public override void OnNetworkSpawn()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadeventCompleted;

        }

        private void SceneManager_OnLoadeventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            print("Welcome!");
            foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform playerTransform = Instantiate(PlayerPrefab);
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
            }
        }
    }
}
