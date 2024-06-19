using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public abstract class _Interactable : NetworkBehaviour
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
