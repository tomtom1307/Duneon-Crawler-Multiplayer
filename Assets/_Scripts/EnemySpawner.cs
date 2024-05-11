using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class EnemySpawner : NetworkBehaviour
    {
        public List<GameObject> enemyPrefabs;
        public float SpawnDelay;
        public float SpawnRange;
        float counter = 0;
        public float spawnAmount = 1;
        bool Spawned = false;
        bool Active = false;   
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
            if (!Active || !IsHost) return;
            if(!Spawned && counter <= spawnAmount)
            {
                counter++;
                Spawned = true;
                float Randomx = Random.Range(-SpawnRange, SpawnRange);
                float Randomz = Random.Range(-SpawnRange, SpawnRange);
                StartCoroutine(SpawnEnemy(Randomx, Randomz));
            }
                
        }
        private void OnTriggerEnter(Collider other)
        {
            Active = true;
        }

        
        
        private IEnumerator SpawnEnemy(float Randomx, float Randomz)
        {
            yield return new WaitForSeconds(SpawnDelay);
            Spawned = false;
            SpawnEnemyServerRpc(Randomx,Randomz);

        }

        [ServerRpc]
        public void SpawnEnemyServerRpc(float Randomx, float Randomz)
        {
            Vector3 RandSpawnPos = new Vector3(transform.position.x + Randomx, transform.position.y, transform.position.z + Randomz);
            var Instance = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], RandSpawnPos, Quaternion.identity);
            var InstanceNetworkObj = Instance.GetComponent<NetworkObject>();
            InstanceNetworkObj.Spawn();
        }




    }
}
