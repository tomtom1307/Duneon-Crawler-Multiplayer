using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SmallDrawerTable : LootGenerator
    {
        Animator anim;
        public List<Transform> LootPositions;
        LootGenerator LootGenerator;
        bool opened = false;
    

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            LootGenerator = GetComponent<LootGenerator>();
        }

        protected override void Interact()
        {
            if(!opened)
            {
                opened = true;
                foreach(Transform loottransform in LootPositions)
                {
                    GameObject loot = GenerateLoot();
                    if(loot != null) {SpawnLoot(loot, loottransform, aschild: true);}
                }
                base.Interact();  
                anim.SetBool("Open", true);
                Prompt = "";
            }
        }

        
    }
}
