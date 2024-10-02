using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class KeyChest : ContainerLogic
    {
        public ItemSpawn itemSpawner;
        void Start()
        {
            base.Start();
        }

        protected override void Interact()
        {
            base.Interact();
            itemSpawner.FloatUp();
        }
    }
}
