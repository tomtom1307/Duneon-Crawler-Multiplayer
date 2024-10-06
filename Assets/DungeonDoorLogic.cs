using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class DungeonDoorLogic : NetworkBehaviour
    {

        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        [ServerRpc(RequireOwnership = false)]
        public void TriggerDoorServerRpc(bool X)
        {
            animator.SetBool("Open", X);
        }

    }
}
