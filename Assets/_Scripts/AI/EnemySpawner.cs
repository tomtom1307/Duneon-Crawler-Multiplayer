using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class EnemySpawner : NetworkBehaviour
    {
        public List<GameObject> enemyPrefabs;
        public BoxCollider Bounds;
        public float SpawnDelay;
        public float SpawnRange;
        public float counter = 0;
        public float EnemiesLeft;
        public float spawnAmount = 1;
        bool Spawned = false;
        public bool Active = false;
        public bool AllEnemiesKilled;
        // Start is called before the first frame update
        void Start()
        {
            Bounds = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!Active || !IsHost) return;
            if(!Spawned && counter < spawnAmount)
            {
                counter++;
                EnemiesLeft++;
                Spawned = true;
                StartCoroutine(SpawnEnemy());
            }
            if (EnemiesLeft == 0 && Active)
            {
                AllEnemiesKilled = true;
            }

        }
        

        
        
        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(SpawnDelay);
            Spawned = false;
            SpawnEnemyServerRpc();

        }

        [ServerRpc]
        public void SpawnEnemyServerRpc()
        {
            Vector3 range = Bounds.size;
            Vector3 RandSpawnPos =  transform.position+ new Vector3(Random.Range(-range.x, range.x), 0, Random.Range(-range.z, range.z));
            var Instance = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], RandSpawnPos, Quaternion.identity);
            Enemy En = Instance.GetComponent<Enemy>();
            En.Spawner = this;
            En.aggression = Mathf.Clamp(0.1f * spawnAmount, 0, 1);
            var InstanceNetworkObj = Instance.GetComponent<NetworkObject>();
            InstanceNetworkObj.Spawn();
        }




    }
}
