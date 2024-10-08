using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using DG.Tweening;

namespace Project
{
    public class Keyhole : _Interactable
    {
        public Animator barricadeAnimator;
        public bool active;
        public string correspondingBarricade;
        GameObject keyObject;
        Material keyMat;
        [SerializeField] Material carvingMat;
        GenericSoundSource soundSource;
        // Start is called before the first frame update
        void Start()
        {
            soundSource = GetComponent<GenericSoundSource>();
            active = true;
            keyObject = transform.GetChild(0).gameObject;
            keyMat = keyObject.GetComponent<Renderer>().material;
            carvingMat = transform.gameObject.GetComponent<Renderer>().materials[2];
            carvingMat.SetFloat("_Tween_Value", 0);
            keyMat.SetFloat("_Tween_Value", 0);
        }

        protected override void Interact()
        {
            if(active)
            {
                if(Inventory.Singleton.ItemIsInInventory($"GateKey{correspondingBarricade}")) //Checks if key is in player inventory
                {
                    PlaceKeyServerRpc();
                    Inventory.Singleton.RemoveInventoryItem($"GateKey{correspondingBarricade}");
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
            DOVirtual.Float(0, 1, 5, val =>
            {
                carvingMat.SetFloat("_Tween_Value", val);
                keyMat.SetFloat("_Tween_Value", val);
            });
            //https://discussions.unity.com/t/help-using-lerp-inside-of-a-coroutine/207869
            barricadeAnimator.SetTrigger($"OpenB{correspondingBarricade}");
        }
        

    }
}