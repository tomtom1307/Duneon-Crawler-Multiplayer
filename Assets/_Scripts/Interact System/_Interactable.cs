using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public abstract class _Interactable : MonoBehaviour
    {
        public string Prompt;


        public void BaseInteract()
        {
            Interact();
        }

        protected virtual void Interact()
        {

        }

        

    }


    

    
}
