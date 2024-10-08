using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class KeyChest : ContainerLogic
    {
        public ItemSpawn itemSpawner;
        [SerializeField] bool unlocked;

        private void OnEnable() => Actions.SpawnerUpdate += UnlockChest;
        private void OnDisable() => Actions.SpawnerUpdate -= UnlockChest;
            
        
        void Start()
        {
            base.Start();
        }

        protected override void Interact()
        {
            if(unlocked)
            {
                base.Interact();
                // Causes key to float up
                itemSpawner.FloatUp();
            }
            else
            {
                anim.SetTrigger("Rustle");
            }
        }

        private void UnlockChest(EnemySpawner enemySpawner, bool start)
        {
            // Checks that spawner stopped
            if(!start)
            {
                // Checks that action came from same barrack
                if(enemySpawner.transform.parent == this.transform.parent) 
                {
                    unlocked = true;
                    anim.SetBool("Padlock Open", true);
                }
            }
        }
    }
}
