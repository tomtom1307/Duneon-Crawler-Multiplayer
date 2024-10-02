using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Project
{
    public class CrystalPickup : _Interactable
    {
        private PlayerStats PS;
        public bool AutoPickup;
        private StatManager WS;
        public PlayerStats.CrystalType CrystalType;


        protected override void Interact()
        {
            base.Interact();
            PS = interacter.GetComponent<PlayerStats>();
            PS.AddCrystals(CrystalType, 1);
            DOTween.Kill(transform); //Kill DOTween before destroying object
            Destroy(this.gameObject);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.WhiteCrystalPickup, 1);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.ClothesRustle, 0.5f);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 10 && AutoPickup)
            {
                interacter = other.gameObject;
                Interact();
            }

        }
    }
}
