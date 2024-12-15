using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;

namespace Project
{
    public class Vase : LootGenerator
    {
        [SerializeField] GameObject BrokenVase;
        DamageableThing DT;
        public float BreakForce;
        GenericSoundSource SoundSource;
        bool Broken;

        public override void Start()
        {
            base.Start();
            DT = GetComponent<DamageableThing>();
            SoundSource = GetComponent<GenericSoundSource>();
            GetComponent<Rigidbody>().isKinematic = false;
        }

        private void Update()
        {
            if (DT.ded && !Broken) {
                BreakClientRpc();
            }
        }

        [ClientRpc]
        private void BreakClientRpc()
        {
            //Handle Loot Generation
            //Note That Generate Loot and SpawnLoot are stored in the inherited class LootGenerator
            GameObject loot = GenerateLoot();
            if(loot != null) { 
                SpawnLoot(loot, transform, autopickup: true);
                AnimationSpawnVFX();
            }


            Broken = true;
            //Pick and Spawn In Whatever Loot possibilities are available 

            //PlaySound
            SoundSource.PlaySound(GenericSoundSource.GenSoundType.BreakSound,1);


            //Replace Current model with broken and give a smash effect.
            var broken = Instantiate(BrokenVase,transform.position,transform.rotation);
            Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                //Give each peice of smashed a force
                rb.AddExplosionForce(BreakForce, transform.position, 1);

                //Handle scaling before destroying so theres no sudden dissapearing parts 
                DOTween.Sequence().SetDelay(1.5f).Append(rb.transform.DOScale(Vector3.zero, 0.3f));


                Destroy(rb.gameObject, 2);
                
            }
            Destroy(broken,3);



            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
