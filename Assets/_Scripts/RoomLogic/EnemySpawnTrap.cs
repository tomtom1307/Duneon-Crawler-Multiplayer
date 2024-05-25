using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class EnemySpawnTrap : Director
    {
        public EnemySpawner spawner;
        public LeverLogic Lever;
        public Animator doorAnim;


        public override void DoSomething()
        {
            spawner.Active = true;
        }


        private void Update()
        {
            if(spawner.AllEnemiesKilled == true)
            {
                OnCompleted();
            }
        }

        public override void OnCompleted()
        {
            doorAnim.SetBool("Open", true);
        }

    }
}
