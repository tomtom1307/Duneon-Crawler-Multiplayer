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

        void Update()
        {
            if (roomSpawner.Active && roomSpawner.CurrentRound<roomSpawner.NumberOfRounds && roomSpawner.EnemiesLeft==0)
            {
                roomSpawner.TriggerNextRound(); //DOESNT FUCKING WORK BECAUSE BARRACK DOOR LOGIC CLASS DESTROYS ITSELF
            }
        }
        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Open", true);
            if(triggerspawn) {BarrackSpawnServerRpc();}
            Destroy(this);
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
