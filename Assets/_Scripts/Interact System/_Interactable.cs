using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public abstract class _Interactable : NetworkBehaviour
    {
        public string Prompt;
        public GameObject interacter;

        public void BaseInteract(GameObject GO = null)
        {
            if(GO != null)
            {
                interacter = GO;
            }
            
            Interact();
        }

        protected virtual void Interact()
        {

        }

        

    }


    

    
}
