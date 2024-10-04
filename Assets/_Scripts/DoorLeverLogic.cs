using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DoorLeverLogic : _Interactable
    {
        Animator anim;
        public DungeonDoorLogic Door;
        public bool Lock;
        [HideInInspector] public int Channel;

        private void Start()
        {
            anim = GetComponent<Animator>();
            //Door = GetComponentInParent<Animator>();
            
        }


        protected override void Interact()
        {
            if (Lock) return;
            base.Interact();
            anim.SetBool("Flip", true);
            Door.TriggerDoor(true);
            Lock = true;
        }
    }
}
