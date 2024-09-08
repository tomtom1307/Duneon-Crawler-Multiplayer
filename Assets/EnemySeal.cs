using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Project
{
    public class EnemySeal : NetworkBehaviour
    {
        public EnemySpawner[] enemySpawners;

        private void OnEnable()
        {
            RoomActions.SpawnerUpdate += ManageDoors;
        }
        private void OnDisable() 
        {
            RoomActions.SpawnerUpdate -= ManageDoors;
        }


        public void ManageDoors(EnemySpawner enemySpawner, bool startorstop)
        {
            
        }
    }
}
