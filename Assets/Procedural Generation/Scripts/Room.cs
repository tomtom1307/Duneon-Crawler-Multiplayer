using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class Room : NetworkBehaviour
    {
        public Vector2 size;
        public GameObject prefab;
        public List<Vector2> ocupied;
        public List<int> DoorPos;
        public List<Vector2> CorridorStartPos;
        public bool Considered;
        NavMeshSurface navmesh;
        List<NetworkObject> NOs;

        private void Start()
        {
            navmesh = GetComponentInChildren<NavMeshSurface>();
            if(navmesh != null )
            {
                navmesh.BuildNavMesh();
            }

            //
             
            //SpawnNetworkObjectServerRpc();
            
        }

        [ServerRpc]
        public void SpawnNetworkObjectServerRpc()
        {
            foreach( NetworkObject obj in NOs )
            {
                obj.Spawn();
            }
        }

        


        public void GridClaimY()
        {

        }


    }
}
