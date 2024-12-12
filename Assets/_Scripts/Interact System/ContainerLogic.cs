using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ContainerLogic : LootGenerator
    {
        public Animator anim;
        public bool LootAsChild;
        public List<Transform> LootPositions;
        bool opened = false;



        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

        }

        protected override void Interact()
        {
            if (!opened)
            {
                opened = true;
                OpenContainerServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void OpenContainerServerRpc()
        {
            OpenContainerClientRpc();
        }

        [ClientRpc]
        public void OpenContainerClientRpc()
        {
            anim.SetBool("Open", true);
            opened = true;
            Prompt = "";
            if (lootTable != null)
            {
                foreach (Transform loottransform in LootPositions)
                {
                    GameObject loot = GenerateLoot();
                    if (loot != null) { SpawnLoot(loot, loottransform, aschild: LootAsChild); }
                }
            }
            base.Interact();
        }

        
    }
}
