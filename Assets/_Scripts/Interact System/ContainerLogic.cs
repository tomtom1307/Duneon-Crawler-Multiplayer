using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ContainerLogic : LootGenerator
    {
        public Animator anim;
        public bool LootAsChild;
        public List<Transform> LootPositions;
        bool opened = false;
        
    

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            
        }

        protected override void Interact()
        {
            if(!opened)
            {
                opened = true;
                Debug.Log(lootTable);
                if(lootTable != null)
                {
                    foreach(Transform loottransform in LootPositions)
                    {
                        GameObject loot = GenerateLoot();
                        if(loot != null) {SpawnLoot(loot, loottransform, aschild: LootAsChild);}
                    }
                }
                base.Interact();
                Debug.Log(anim);
                anim.SetBool("Open", true);
                Prompt = "";
            }
        }

        
    }
}
