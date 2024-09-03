using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

namespace Project
{
    public class BarrackDoorLogic : _Interactable
    {
        [SerializeField] EnemySpawner roomSpawner;
        Animator anim;
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        
        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
            BarrackSpawnServerRpc();
            Destroy(this);
        }

        [ServerRpc(RequireOwnership=false)]
        void BarrackSpawnServerRpc()
        {
            roomSpawner.Active = true;
        }
    }
}
