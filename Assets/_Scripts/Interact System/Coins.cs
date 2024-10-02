using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Project
{
    public class Coins : _Interactable
    {
        private PlayerStats PS;
        private StatManager WS;

        public int minamount = 2;
        public int maxamount = 50;
        public bool AutoPickup;
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
            DOTween.Kill(transform); //Kill DOTween before destroying object
            Destroy(this.gameObject);
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.GoldPickupSound, 1);
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
