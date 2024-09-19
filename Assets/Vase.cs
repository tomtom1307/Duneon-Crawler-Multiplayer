using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

namespace Project
{
    public class Vase : MonoBehaviour
    {
        [SerializeField] GameObject BrokenVase;
        DamageableThing DT;
        public float BreakForce;
        GenericSoundSource SoundSource;
        bool Broken;

        private void Start()
        {
            DT = GetComponent<DamageableThing>();
            SoundSource = GetComponent<GenericSoundSource>();
        }

        private void Update()
        {
            if (DT.ded && !Broken) { 
                Break();
            }
        }

        private void Break()
        {
            Broken = true;
            //Pick and Spawn In Whatever Loot possibilities are available 

            //PlaySound
            SoundSource.PlaySound(GenericSoundSource.GenSoundType.BreakSound,1);


            //Replace Current model with broken and give a smash effect.
            var broken = Instantiate(BrokenVase,transform.position,transform.rotation);
            Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(BreakForce, transform.position, 1);
                DOTween.Sequence().SetDelay(1.5f).Append(rb.transform.DOScale(Vector3.zero, 0.3f));
                Destroy(rb.gameObject, 2);
                
            }
            Destroy(broken,3);



            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
