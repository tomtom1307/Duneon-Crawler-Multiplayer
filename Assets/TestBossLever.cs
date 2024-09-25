using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TestBossLever : _Interactable
    {


        public EnemySpawner _enemySpawner;
        protected override void Interact()
        {
            base.Interact();
            GetComponent<Animator>().SetBool("Flip",true);
            _enemySpawner.StartSpawner();

        }
    }
}
