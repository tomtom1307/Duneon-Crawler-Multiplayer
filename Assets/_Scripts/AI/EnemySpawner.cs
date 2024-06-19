using System;
using System.Collections;
using System.Collections.Generic;
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
        public float AmountSpawned;
        public float TotalEnemies;
        public Enemies[] enemies;
        public GameObject currentSpawn;
        public float spawnAmount = 1;
        bool Spawned = false;
        public bool Active = false;
        public bool AllEnemiesKilled;
        // Start is called before the first frame update
        void Start()
        {
            Bounds = GetComponent<BoxCollider>();
            foreach(var Enemy in enemies)
            {
                TotalEnemies += Enemy.Amount;
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!Active || !IsHost) return;

            foreach (var enemy in enemies)
            {
                if (AmountSpawned >= TotalEnemies) break;
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



            if (EnemiesLeft == 0 && Active)
            {
                AllEnemiesKilled = true;
            }

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
                SpawnEnemyWithPositionServerRpc(Position.position, Position.localRotation);
            }
            

        }

        [ServerRpc]
        public void SpawnEnemyServerRpc()
        {
            Vector3 range = Bounds.size;
            Vector3 RandSpawnPos =  transform.position+ new Vector3(UnityEngine.Random.Range(-range.x, range.x), 0, UnityEngine.Random.Range(-range.z, range.z));
            var Instance = Instantiate(currentSpawn, RandSpawnPos, Quaternion.identity);
            Enemy En = Instance.GetComponent<Enemy>();
            En.Spawner = this;
            
            
            //En.aggression = Mathf.Clamp(0.1f * spawnAmount, 0, 1);
            var InstanceNetworkObj = Instance.GetComponent<NetworkObject>();
            InstanceNetworkObj.Spawn();
            
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
            public List<Transform> Position;
        }




    }
}
