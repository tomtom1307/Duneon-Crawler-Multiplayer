using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SmallDrawerTable : _Interactable
    {
        Animator anim;
        public List<Transform> LootPositions;
        LootGenerator LootGenerator;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            LootGenerator = GetComponent<LootGenerator>();
        }

        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
            foreach (Transform t in LootPositions)
            {
                
            }

            Prompt = "";
        }

        
    }
}
