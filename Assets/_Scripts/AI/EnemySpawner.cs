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
        public float spawnDelay;
        public float TotalEnemies;
        public int NumberOfRounds;
        public int CurrentRound;
        public List<Enemies> enemies;
        bool Spawned = false;
        public bool Active = false;
        GameManager gameManager;
        public bool AllEnemiesKilled;
        public bool AutoRoundTrigger;


        List<GameObject> Prefabs = new List<GameObject> { };
        // Start is called before the first frame update
        void Start()
        {
            Actions.Initialize();
            gameManager = GameManager.instance;
            print(gameManager);
            CurrentRound = 1;
            Bounds = GetComponent<BoxCollider>();
            NumberOfRounds = FindTotalNumberOfRounds();
            TotalEnemies = CalculateTotalNumberOfEnemies(1);

            foreach (Enemies item in enemies)
            {
                Prefabs.Add(item.Prefab);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (!Active || !IsHost) return;



            int i = 0;
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


                    if(enemy.Position.Count > 0 && !enemy.RandomSpawn)
                    {
                        int Random = UnityEngine.Random.Range(0, enemy.Position.Count);
                        pos = enemy.Position[Random];
                        enemy.Position.Remove(enemy.Position[Random]);
                    }
                    StartCoroutine(SpawnEnemy(enemy.RandomSpawn, pos, i));
                    
                }
                i++;
            }
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


        }
        
        public void SpawnerEnd()
        {
            print(Actions.SpawnerUpdate);
            if(Actions.SpawnerUpdate != null)
            {
                Actions.SpawnerUpdate(this, false);
            }
            
            
        }


        public void StartSpawner()
        {
            



            Active = true;
            Actions.SpawnerUpdate(this, true);
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
        
        private IEnumerator SpawnEnemy(bool Random, Transform Position, int index)
        {

            yield return new WaitForSeconds(SpawnDelay);
            Spawned = false;
            

            Debug.Log(Position);
            if (Random || Position.localPosition == Vector3.zero)
            {
                print("doing Random Spawn");
                RandomPositionSpawnServerRpc(index);
            }
            else
            {
                StartCoroutine(DelayedSpawn(Position.position, Position.rotation, GameManager.SpawnDelay, index));
            }
        }

        [ServerRpc]
        public void RandomPositionSpawnServerRpc(int index)
        {
            Vector3 range = 0.5f*Bounds.size;
            Vector3 RandSpawnPos =  transform.position+ new Vector3(UnityEngine.Random.Range(-range.z, range.z), 0, UnityEngine.Random.Range(-range.x, range.x));

            StartCoroutine(DelayedSpawn(RandSpawnPos, Quaternion.identity, GameManager.SpawnDelay, index));
        }

        public IEnumerator DelayedSpawn(Vector3 pos, Quaternion rot, float delay, int index)
        {
            GameManager.instance.DoSpawnEffect(pos, Quaternion.identity);
            yield return new WaitForSeconds(delay);
            SpawnEnemyWithPositionServerRpc(pos, rot, index);
        }

        [ServerRpc]
        public void SpawnEnemyWithPositionServerRpc(Vector3 pos, Quaternion rot, int index)
        {
            GameObject currentSpawn = Prefabs[index];
            var Instance = Instantiate(currentSpawn, pos, rot);
            Enemy En = Instance.GetComponent<Enemy>();
            En.Spawner = this;
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


        


        

        [Serializable]
        public struct Enemies
        {
            public string name;
            public GameObject Prefab;
            public int Amount;
            public bool RandomSpawn;
            public int[] Rounds;
            public List<Transform> Position;
            

            public Enemies(string name, GameObject prefab, int amount, bool randomSpawn, int[] rounds, List<Transform> position)
            {
                this.name = name;
                Prefab = prefab;
                Amount = amount;
                RandomSpawn = randomSpawn;
                Rounds = rounds;
                Position = position;
            }
        }

    }
}
