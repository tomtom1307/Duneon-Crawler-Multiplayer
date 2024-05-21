using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class GameManager : NetworkBehaviour
    {
        public static GameManager instance;
        public List<PlayerStats> playerStats = new List<PlayerStats>();
        [SerializeField] private Transform PlayerPrefab;

        private void Start()
        {
            if (instance==null)
            {
                instance = this;
            }
            
            GetPlayerStats();
        }


        
        public void GetPlayerStats()
        {
            playerStats.Clear();
            var connectedClients = NetworkManager.Singleton.ConnectedClientsList;
            foreach (var client in connectedClients)
            {
                playerStats.Add(client.PlayerObject.gameObject.GetComponent<PlayerStats>());
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void AwardXPServerRpc(int xp) 
        {
            print("Award on Kill");
            GiveMana(50);
            foreach (var stats in playerStats)
            {
                print("Added Xp");
                stats.AddXpClientRpc(xp);
                stats.AddGoldClientRpc(5);
                
            }
            
        }

       
        public void GiveMana(float mana)
        {
            foreach (var stats in playerStats)
            {
                stats.AddManaClientRpc(mana);
            }
        }


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
                playerStats.Add(playerTransform.gameObject.GetComponent<PlayerStats>());
                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
            }
        }
    }
}
