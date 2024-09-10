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
        public bool triggerspawn = true;
        
        void Start()
        {
            anim = GetComponent<Animator>();
        }
        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
            if(triggerspawn) {BarrackSpawnServerRpc();}
            Prompt = "";
            //Destroy(this);
        }

        [ServerRpc(RequireOwnership=false)]
        void BarrackSpawnServerRpc()
        {
            TriggerSpawner();
            roomSpawner.Active = true;
        }
        public void TriggerSpawner()
        {
            RoomActions.SpawnerUpdate(roomSpawner,true);
        }
    }
}
