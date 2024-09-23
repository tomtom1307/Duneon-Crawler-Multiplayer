using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ContainerLogic : LootGenerator
    {
        Animator anim;
        public bool LootAsChild;
        public List<Transform> LootPositions;
        bool opened = false;
        
    

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            
        }

        protected override void Interact()
        {
            if(!opened)
            {
                opened = true;
                foreach(Transform loottransform in LootPositions)
                {
                    GameObject loot = GenerateLoot();
                    if(loot != null) {SpawnLoot(loot, loottransform, aschild: LootAsChild);}
                }
                base.Interact();  
                anim.SetBool("Open", true);
                Prompt = "";
            }
        }

        
    }
}
