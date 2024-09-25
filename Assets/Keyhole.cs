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
        GameObject keyObject;
        Renderer crystalRend;
        public Material dimRed;
        public Material glowingRed;
        Material[] holematerials;
        // Start is called before the first frame update
        void Start()
        {
            active = true;
            keyObject = transform.GetChild(0).gameObject;
            crystalRend = keyObject.GetComponent<Renderer>();
            holematerials = transform.gameObject.GetComponent<Renderer>().materials;
            holematerials[2] = glowingRed;
        }

        protected override void Interact()
        {
            if(active)
            {

                active=false;
                Prompt = "";
                //play sound
                keyObject.SetActive(true);
                //Crystalrend.material.Lerp(dimRed, glowingRed,t);
                crystalRend.material = glowingRed;
                transform.gameObject.GetComponent<Renderer>().materials = holematerials; //LERP THIS AND THE CRYSTAL MATERIAL IN A COROUTINE
                //https://discussions.unity.com/t/help-using-lerp-inside-of-a-coroutine/207869
                barricadeAnimator.SetTrigger("OpenB" + correspondingBarricade);
            }
        }
    }
}