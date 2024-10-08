using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class AbilityPageBook : _Interactable
    {

        public Canvas Canvas;

        

        private void Start()
        {
            Canvas = GetComponentInChildren<Canvas>();
            Canvas.enabled = false;
        }

        protected override void Interact()
        {
            base.Interact();
            Canvas.enabled = true;

            //Disable Camera Movement
            Camera.main.GetComponent<PlayerCam>().enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Canvas.enabled = false;
                Camera.main.GetComponent<PlayerCam>().enabled = true;
            }

        }


    }
}
