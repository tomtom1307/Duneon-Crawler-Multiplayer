using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
            Lock = true;
            base.Interact();
            OpenDoorServerRpc();
            Door.TriggerDoorServerRpc(true);

        }

        [ServerRpc(RequireOwnership =false)]
        public void OpenDoorServerRpc()
        {
            
            anim.SetBool("Flip", true);
            OpenDoorClientRpc();
        }

        [ClientRpc]
        public void OpenDoorClientRpc()
        {
            
            Lock = true;
        }
    }
}
