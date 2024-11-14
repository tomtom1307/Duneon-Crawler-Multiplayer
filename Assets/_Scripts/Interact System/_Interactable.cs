using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public abstract class _Interactable : NetworkBehaviour
    {
        public string Prompt = "[F]";
        public bool isActive;
        [HideInInspector] public GameObject interacter;

        private void Start()
        {
            isActive = true;
        }



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
            InteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void InteractServerRpc()
        {
            InteractClientRpc();
        }

        [ClientRpc]
        public void InteractClientRpc()
        {
            Prompt = "";
        }
        

    }


    

    
}
