using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SmallDrawerTable : _Interactable
    {
        Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();

        }

        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
        }

        public struct Loot
        {
            public float MaxAmount;
            public GameObject model;
            public float Likelihood;
        }
    }
}
