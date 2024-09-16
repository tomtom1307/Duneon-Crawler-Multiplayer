using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Coins : _Interactable
    {
        private PlayerStats PS;
        private StatManager WS;

        public int minamount = 2;
        public int maxamount = 50;
        private int amount;
        private void Start()
        {
            amount = Random.Range(minamount, maxamount);
        }

        protected override void Interact()
        {
            base.Interact();
            PS = interacter.GetComponent<PlayerStats>();
            PS.AddGoldClientRpc(amount);
            Destroy(this.gameObject);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.GoldPickupSound, 1);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.ClothesRustle, 0.5f);
        }
    }
}
