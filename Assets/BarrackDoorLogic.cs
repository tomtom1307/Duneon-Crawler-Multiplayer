using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class BarrackDoorLogic : _Interactable
    {
        Animator anim;
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        
        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
            Destroy(this);
        }
    }
}
