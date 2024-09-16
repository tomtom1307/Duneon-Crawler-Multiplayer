using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class CrystalPickup : _Interactable
    {
        private PlayerStats PS;
        private StatManager WS;
        public PlayerStats.CrystalType CrystalType;


        protected override void Interact()
        {
            base.Interact();
            PS = interacter.GetComponent<PlayerStats>();
            PS.AddCrystals(CrystalType, 1);
            Destroy(this.gameObject);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.WhiteCrystalPickup, 1);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.ClothesRustle, 0.5f);
        }
    }
}
