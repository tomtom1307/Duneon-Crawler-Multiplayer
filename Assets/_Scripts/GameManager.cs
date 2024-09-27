
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

        public GameObject SpawnEffect;
        public static float SpawnDelay = 1.5f;
        public bool initialized = false;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            if (instance==null)
            {
                instance = this;
            }

            //Check Players connected
            Invoke("GetPlayerStats",3f);
        }

        




        public void GetPlayerStats()
        {
            playerStats.Clear();
            if (!IsServer)
            {
                return;
            }
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

        GameObject SpawnObj;
        public void TriggerSpawn(GameObject Go, Vector3 pos, Quaternion rot, Vector3 finalPos)
        {
            print("Spawned");
            SpawnObj = Go;  
            SpawnObjectServerRpc(pos, rot, finalPos);
        }


        public void DoSpawnEffect(Vector3 pos,  Quaternion rot)
        {
            SpawnObj = SpawnEffect;
            SpawnObjectServerRpc(pos, rot, pos);
        }


        [ServerRpc(RequireOwnership =false)]
        public void SpawnObjectServerRpc(Vector3 pos, Quaternion rot, Vector3 finalPos)
        {
            var SpawnedObj = Instantiate(SpawnObj, pos, rot);
            NetworkObject NO = SpawnedObj.GetComponent<NetworkObject>();
            if(NO != null) { NO.Spawn();
                Invoke("NO.Despawn", 5);
            }
            else
            {
                Destroy(SpawnedObj,5);
            }

            
            MoveToTarget MT;
            if(SpawnedObj.TryGetComponent<MoveToTarget>(out MT))
            {
                MT.target = finalPos;
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
            print("SpawnedGameManager");
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadeventCompleted;

        }

        private void SceneManager_OnLoadeventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            
            print("Welcome!");
            if(!initialized)
            {
                initialized = true;
                foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    
                    Transform playerTransform = Instantiate(PlayerPrefab);
                    playerStats.Add(playerTransform.gameObject.GetComponent<PlayerStats>());
                    playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
                }
            }
            else
            {
                GameObject.Find("SpawnManager").GetComponent<PlayerSpawner>().enabled=true;
            }
            
        }
    }
}
