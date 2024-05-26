using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class FlipableTable : _Interactable
    {
        Animator anim;
        private void Start()
        {
            anim = GetComponent<Animator>();
        }


        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Flip", true);

            Destroy(this);
        }
    }
}
