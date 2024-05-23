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


        public void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.2f);
            
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
            Destroy(GO, 1);
            Destroy(this.gameObject);
        }

    }
}
