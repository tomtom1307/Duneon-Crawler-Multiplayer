using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Project
{
    public class EnemySeal : NetworkBehaviour
    
    {
        public EnemySpawner[] enemySpawners;
        public int spawnersActive=0;
        public bool Open=true;
        public Animator anim;

        private void OnEnable()
        {
            RoomActions.SpawnerUpdate += ManageDoors;
        }
        private void OnDisable() 
        {
            RoomActions.SpawnerUpdate -= ManageDoors;
        }


        public void ManageDoors(EnemySpawner enemySpawner, bool start)
        {
            if(enemySpawner.transform.parent.gameObject.transform.parent.gameObject == this.transform.parent.gameObject) //checks if action came from the same MainHall
            {
                if(start)
                {
                    spawnersActive += 1;
                }
                else
                {
                    spawnersActive -= 1;
                }

                if(spawnersActive != 0 && Open)
                {
                    Open = false;
                }
                else if(spawnersActive == 0 && !Open)
                {
                    Open = true;
                }
            }
        }
    }
}
