using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DoorLeverLogic : _Interactable
    {
        Animator anim;
        public Animator Door;
        [HideInInspector] public int Channel;

        private void Start()
        {
            anim = GetComponent<Animator>();
            //Door = GetComponentInParent<Animator>();
            
        }


        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Flip", true);
            Door.SetBool("Open", true);

            this.enabled = false;
        }
    }
}
