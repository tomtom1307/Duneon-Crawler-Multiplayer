
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
        public List<PlayerStats> playerStats;
        [SerializeField] private Transform PlayerPrefab;

        public GameObject SpawnEffect;
        public NetworkVariable<int> NumberOfPlayers;
        public static float SpawnDelay = 1.5f;
        public bool initialized = false;

        private void Start()
        {
            
            playerStats = new List<PlayerStats>();

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
            
            if (!IsServer)
            {
                return;
            }
            playerStats.Clear();
            var connectedClients = NetworkManager.Singleton.ConnectedClientsList;
            foreach (var client in connectedClients)
            {
                NumberOfPlayers.Value++;
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


        public void DoSpawnEffect(Vector3 pos,  Quaternion rot, float size)
        {
            SpawnObj = SpawnEffect;
            SpawnObjectServerRpc(pos, rot, pos, size);
        }


        [ServerRpc(RequireOwnership =false)]
        public void SpawnObjectServerRpc(Vector3 pos, Quaternion rot, Vector3 finalPos, float size = 0)
        {
            var SpawnedObj = Instantiate(SpawnObj, pos, rot);
            if(SpawnedObj.TryGetComponent<ChaosSpawnFX>(out ChaosSpawnFX fx))
            {
                SpawnedObj.transform.localScale = size * transform.localScale;
            }
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
