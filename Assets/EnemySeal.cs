using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Project
{
    public class EnemySeal : NetworkBehaviour
    
    {
        public int spawnersActive=0;
        private bool Open;
        public Animator anim;

        private void OnEnable()
        {
            RoomActions.SpawnerUpdate += ManageDoors;
            anim.SetBool("Open",true);
            Open = true;
        }
        private void OnDisable() 
        {
            RoomActions.SpawnerUpdate -= ManageDoors;
        }


        public void ManageDoors(EnemySpawner enemySpawner, bool start)
        {
            Debug.Log("Action listened :)");
            if(enemySpawner.transform.parent.gameObject.transform.parent.gameObject == this.transform.parent.gameObject) //checks if action came from the same MainHall
            {
                Debug.Log("Main Hall verified");
                if(start)
                {
                    Debug.Log("Active spawner registered");
                    spawnersActive += 1;
                }
                else
                {
                    spawnersActive -= 1;
                }

                if(spawnersActive != 0 && Open)
                {
                    anim.SetBool("Open",false);
                }
                else if(spawnersActive == 0 && !Open)
                {
                    anim.SetBool("Open",true);
                }
                Open = anim.GetBool("Open");
            }
        }
    }
}
