using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Project
{
    public class EnemySpawner : NetworkBehaviour
    {
        public BoxCollider Bounds;
        public float SpawnDelay;
        public float SpawnRange;
        public float counter = 0;
        public float EnemiesLeft;
        public bool InfiniteWaves;
        public float AmountSpawned;
        public float TotalEnemies;
        public int NumberOfRounds;
        public int CurrentRound;
        public Enemies[] enemies;
        public GameObject currentSpawn;
        public float spawnAmount = 1;
        bool Spawned = false;
        public bool Active = false;
        public bool AllEnemiesKilled;
        public bool AutoRoundTrigger;
        // Start is called before the first frame update
        void Start()
        {
            CurrentRound = 1;
            Bounds = GetComponent<BoxCollider>();
            NumberOfRounds = FindTotalNumberOfRounds();
            TotalEnemies = CalculateTotalNumberOfEnemies(1);
        }

        // Update is called once per frame
        void Update()
        {
            if (!Active || !IsHost) return;
            if (EnemiesLeft == 0 && CurrentRound == NumberOfRounds)
            {
                AllEnemiesKilled = true;
                if (InfiniteWaves)
                {
                    AmountSpawned = 0;
                    TotalEnemies += 5;
                }
                else
                {
                    Active=false;
                    SpawnerEnd();
                }
            }

            
            if (AutoRoundTrigger && EnemiesLeft == 0 && CurrentRound < NumberOfRounds) TriggerNextRound();
            foreach (var enemy in enemies)
            {
                if (AmountSpawned >= TotalEnemies) break;
                

                if (!enemy.Rounds.Contains(CurrentRound))
                {
                    continue;
                }
                counter = 0;
                while (counter < enemy.Amount)
                {
                    
                    counter++;
                    AmountSpawned++;
                    EnemiesLeft++;
                    
                    Spawned = true;
                    Transform pos = transform;
                    if(enemy.Position.Count > 0)
                    {
                        int Random = UnityEngine.Random.Range(0, enemy.Position.Count);
                        pos = enemy.Position[Random];
                        enemy.Position.Remove(enemy.Position[Random]);
                    }
                    StartCoroutine(SpawnEnemy(enemy.RandomSpawn, pos, enemy.Prefab));
                }
            }


        }
        
        public void SpawnerEnd()
        {
            RoomActions.SpawnerUpdate(this,false);
        }
        public int FindTotalNumberOfRounds()
        {
            int totalNumberOfRounds = 0;
            int i = 0;
            while (totalNumberOfRounds + 5 > i)
            {
                foreach (var Enemy in enemies)
                {
                    if (!Enemy.Rounds.Contains(i)) continue;
                    totalNumberOfRounds = i;


                }
                i++;
            }


            return totalNumberOfRounds;
        }

        public int CalculateTotalNumberOfEnemies(int round)
        {
            int EN = 0;
            foreach (var Enemy in enemies)
            {
                if (!Enemy.Rounds.Contains(round)) continue;
                EN += Enemy.Amount;

            }
            return EN;
        }
        
        private IEnumerator SpawnEnemy(bool Random, Transform Position, GameObject prefab)
        {

            yield return new WaitForSeconds(SpawnDelay);
            Spawned = false;
            currentSpawn = prefab;
            if (Random|| Position.localPosition == Vector3.zero)
            {
                SpawnEnemyServerRpc();
            }
            else
            {
                SpawnEnemyWithPositionServerRpc(Position.position, Position.rotation);
            }
            

        }

        [ServerRpc]
        public void SpawnEnemyServerRpc()
        {
            Vector3 range = 0.5f*Bounds.size;
            Vector3 RandSpawnPos =  transform.position+ new Vector3(UnityEngine.Random.Range(-range.x, range.x), 0, UnityEngine.Random.Range(-range.z, range.z));
            var Instance = Instantiate(currentSpawn, RandSpawnPos, Quaternion.identity);
            Enemy En = Instance.GetComponent<Enemy>();
            En.Spawner = this;
            
            
            //En.aggression = Mathf.Clamp(0.1f * spawnAmount, 0, 1);
            var InstanceNetworkObj = Instance.GetComponent<NetworkObject>();
            InstanceNetworkObj.Spawn();
            
        }

        public void TriggerNextRound()
        {
            AmountSpawned = 0;
            
            CurrentRound += 1;
            TotalEnemies = CalculateTotalNumberOfEnemies(CurrentRound);
            Active = true;
            
        }


        [ServerRpc]
        public void SpawnEnemyWithPositionServerRpc(Vector3 pos, Quaternion rot)
        {
            
            var Instance = Instantiate(currentSpawn, pos, rot);
            Enemy En = Instance.GetComponent<Enemy>();
            En.Spawner = this;
            //En.aggression = Mathf.Clamp(0.1f * spawnAmount, 0, 1);
            var InstanceNetworkObj = Instance.GetComponent<NetworkObject>();
            InstanceNetworkObj.Spawn();

        }

        [Serializable]
        public struct Enemies
        {
            public string name;
            public GameObject Prefab;
            public int Amount;
            public bool RandomSpawn;
            public int[] Rounds;
            public List<Transform> Position;
        }

    }
}
