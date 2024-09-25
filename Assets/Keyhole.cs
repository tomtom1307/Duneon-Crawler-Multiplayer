using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Keyhole : _Interactable
    {
        public Animator barricadeAnimator;
        public bool active;
        public string correspondingBarricade;
        // Start is called before the first frame update
        void Start()
        {
            active = true;
        }

        protected override void Interact()
        {
            if(active)
            {

                active=false;
                barricadeAnimator.SetTrigger("OpenB" + correspondingBarricade);
                Prompt = "";
                //Change material of inset to glowing one
                //play sound
            }
        }
    }
}