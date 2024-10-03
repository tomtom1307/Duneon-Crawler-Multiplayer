using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

namespace Project
{
    public class Keyhole : _Interactable
    {
        public Animator barricadeAnimator;
        public bool active;
        public string correspondingBarricade;
        GameObject keyObject;
        Renderer crystalRend;
        public Material dimRed;
        public Material glowingRed;
        Material[] holematerials;
        GenericSoundSource soundSource;
        // Start is called before the first frame update
        void Start()
        {
            soundSource = GetComponent<GenericSoundSource>();
            active = true;
            keyObject = transform.GetChild(0).gameObject;
            crystalRend = keyObject.GetComponent<Renderer>();
            holematerials = transform.gameObject.GetComponent<Renderer>().materials;
            holematerials[2] = glowingRed;
        }

        protected override void Interact()
        {
            if(active)
            {
                if(Inventory.Singleton.ItemIsInInventory($"GateKey{correspondingBarricade}")) //Checks if key is in player inventory
                {
                    active=false;
                    Prompt = "";
                    //play sound
                    soundSource.PlaySound(GenericSoundSource.GenSoundType.KeySlotting, 1);

                    keyObject.SetActive(true);
                    //Crystalrend.material.Lerp(dimRed, glowingRed,t);
                    crystalRend.material = glowingRed;
                    transform.gameObject.GetComponent<Renderer>().materials = holematerials; //LERP THIS AND THE CRYSTAL MATERIAL IN A COROUTINE
                    //https://discussions.unity.com/t/help-using-lerp-inside-of-a-coroutine/207869
                    barricadeAnimator.SetTrigger($"OpenB{correspondingBarricade}");
                }
                else
                {
                    //TODO: Include message prompt saying "You do not have the correct key."
                }
            }
        }
        [ServerRpc(RequireOwnership=false)]
        public void PlaceKeyServerRpc()
        {
            active=false;
            Prompt = "";
            //play sound
            soundSource.PlaySound(GenericSoundSource.GenSoundType.KeySlotting, 1);

            keyObject.SetActive(true);
            //Crystalrend.material.Lerp(dimRed, glowingRed,t);
            crystalRend.material = glowingRed;
            transform.gameObject.GetComponent<Renderer>().materials = holematerials; //LERP THIS AND THE CRYSTAL MATERIAL IN A COROUTINE
            //https://discussions.unity.com/t/help-using-lerp-inside-of-a-coroutine/207869
            barricadeAnimator.SetTrigger($"OpenB{correspondingBarricade}");
        }
        

    }
}