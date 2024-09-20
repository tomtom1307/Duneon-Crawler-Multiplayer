using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class MoveToTarget : NetworkBehaviour
    {
        public Vector3 target;
        public float speed;
        public GameObject onDestroySpawn;
        public bool Flag = false;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        Vector3 dir;
        private void Start()
        {
            dir = (target - transform.position).normalized
        }

        public void Update()
        {
            transform.position = Vector3.Lerp(transform.position, 40*dir, speed*Time.deltaTime);
            
            if (Vector3.Distance(transform.position, target) < 1 ) 
            {
                
                if(onDestroySpawn!= null && !Flag)
                {
                    Flag = true;
                    SpawnExplosionServerRpc();
                }
                else
                {
                    Destroy(this.gameObject);
                }
                
                
                
            
            }
        }

        [ServerRpc(RequireOwnership =false)]
        public void SpawnExplosionServerRpc()
        {
            var bum = Instantiate(onDestroySpawn, transform.position, Quaternion.identity);
            bum.GetComponent<NetworkObject>().Spawn();
            StartCoroutine(Delete(bum));
            
        }


        
        IEnumerator Delete(GameObject GO)
        {
            yield return new WaitForSeconds(5);
            GO.GetComponent<NetworkObject>().Despawn();
            
            Destroy(this.gameObject);

        }

    }
}
