using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

namespace Project
{
    public class WoodenDoorSpawner : _Interactable
    {
        [SerializeField] EnemySpawner roomSpawner;
        Animator anim;
        public bool triggerspawn = true;
        PlayerCheck playerCheck;
        GenericSoundSource soundSource;

        void Start()
        {
            soundSource = GetComponent<GenericSoundSource>();
            anim = GetComponent<Animator>();
            playerCheck = transform.parent.parent.parent.gameObject.GetComponent<PlayerCheck>();
        }
        protected override void Interact()
        {
            if (!anim.GetBool("Open"))
            {
                base.Interact();
                anim.SetBool("Open", true);
                DoorSoundClientRpc();
                if (triggerspawn) { triggerspawn = false; BarrackSpawnServerRpc(); }
                Prompt = "";
                //Destroy(this);

            }


        }

        [ServerRpc(RequireOwnership = false)]
        void BarrackSpawnServerRpc()
        {
            if (!roomSpawner.Active)
            {
                TriggerSpawner();
                roomSpawner.Active = true;
            }
        }
        public void TriggerSpawner()
        {
            // Actions.SpawnerUpdate(roomSpawner, true);
            roomSpawner.Active = true;
            Debug.Log("yes");
        }
        [ClientRpc]
        void DoorSoundClientRpc()
        {
            soundSource.PlaySound(GenericSoundSource.GenSoundType.DoorOpen, 1);
        }
    }
}
