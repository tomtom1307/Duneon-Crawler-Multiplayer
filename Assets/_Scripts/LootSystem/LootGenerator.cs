using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEditor.Progress;

namespace Project
{
    public class LootGenerator : _Interactable
    {
        //List to store all possible loot options with their respective drop chance
        public LootTable lootTable;
        public bool LootVFX;
        public List<Transform> SpawnedLoot;
        public List<VFX> VFXcolor;
        public float JumpRandomness;
        public Vector3 JumpDir;
        public float JumpDelay = 0.7f;
        public enum VFX
        {
            white,
            green,
            blue,
            purple,
            orange
        }

        private Dictionary<VFX, GameObject> VFXMap;

        GenericSoundSource GSS;


        public virtual void Start()
        {
            GSS = GetComponent<GenericSoundSource>();
            if (lootTable != null)
            {
                VFXMap = new Dictionary<VFX, GameObject>
                {
                    { VFX.white, lootTable.whiteVFXPrefab },
                    { VFX.green, lootTable.greenVFXPrefab },
                    { VFX.blue, lootTable.blueVFXPrefab },
                    { VFX.purple, lootTable.purpleVFXPrefab },
                    { VFX.orange, lootTable.orangeVFXPrefab }
                };
            }

            
        }

        public GameObject GetVFXPrefab(VFX Color)
        {
            
            if (VFXMap.TryGetValue(Color, out GameObject prefab))
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab not found for color: " + Color);
                return null;
            }
        }

        VFX color;
        //Generates Loot and Returns the prefab of the generated loot Gameobject.
        public virtual GameObject GenerateLoot()
        {
            /// WARNING WILL RETURN NULL SO YOU NEED TO HANDLE THIS WHEN IMPLIMENTING ANYTHING WITH THIS FUNCTION


            //Generate random number (101 bc Range is exclusive)
            float rng = UnityEngine.Random.Range(0, 101);

            //make list to store all possible items
            List<LootItem> possibleItems = new List<LootItem>();

            //Loop through Lootitems checking the drop chance and if the rng is less than store it in our list
            foreach (LootItem item in lootTable.List)
            {
                if(rng <= item.dropChance)
                {
                    
                    possibleItems.Add(item);
                }
            }

            
            if(possibleItems.Count > 0 )
            {
                //Maybe check the rarest item and give the 
                LootItem droppedItem = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count)];
                color = droppedItem.VFXColor;
                VFXcolor.Add(color);
                return droppedItem.prefab;

                
            }

            else
            {
                return null;
            }
        }


        //Handle Spawning given a transform 
        public virtual void SpawnLoot(GameObject loot_prefab, Transform target, bool Inherit_Rotation = false, bool aschild = false, bool autopickup = false)
        {
            GameObject LootItem;
            Quaternion rot;
            if (Inherit_Rotation)
            {
                rot = target.rotation;
            }
            else rot = Quaternion.identity;


            

            if(aschild)
            {
                LootItem = Instantiate(loot_prefab, target);
            }
            else
            {
                LootItem = Instantiate(loot_prefab, target.position, rot);
            }
            //guaranteeing loot is autopickup if flag is true
            if (autopickup)
            {
                //Verifying that loot has coin or crystal component
                if(LootItem.GetComponent<Coins>()!=null)
                {LootItem.GetComponent<Coins>().AutoPickup = true;}
                
                else if(LootItem.GetComponent<CrystalPickup>()!=null)
                {LootItem.GetComponent<CrystalPickup>().AutoPickup = true;}
            }
            
            if (LootVFX)
            {
                //SpawnVFX(LootItem.transform);
                Debug.Log("LootItemVFX");
                SpawnedLoot.Add(LootItem.transform);
            }
        }

        //Handle Spawning given a position and a rotation 
        public virtual void SpawnLoot(GameObject loot_prefab, Vector3 pos, Quaternion rot )
        {
            GameObject lootitem = Instantiate(loot_prefab, pos, rot);
            lootitem.GetComponent<_Interactable>();

            if (LootVFX)
            {
                //SpawnVFX(LootItem.transform);
                Debug.Log("LootItemVFX");
                SpawnedLoot.Add(lootitem.transform);
            }

        }

        //For animation event so jump animation doesnt happen immediately
        public void AnimationSpawnVFX()
        {
            int i = 0;
            foreach (var item in SpawnedLoot)
            {
                SpawnVFX(item, VFXcolor[i]);
                i++;
            }
        }



        public void SpawnVFX(Transform item, VFX color)
        {
            var VFX = Instantiate(GetVFXPrefab(color), item);

            //Calculate Jump Direction 

            Vector3 jumpDir = (JumpDir)+new Vector3(UnityEngine.Random.Range(-JumpRandomness, JumpRandomness), 0, UnityEngine.Random.Range(-JumpRandomness, JumpRandomness));

            //DO Jump
            float delay = UnityEngine.Random.Range(0, JumpDelay);
            DOTween.Sequence().SetDelay(delay).Append(item.transform.DOJump(item.transform.position + 1.5f * jumpDir.normalized, 2, 1, 1f));
            Invoke(nameof(JumpSound), delay);
            //Append(item.transform.DOMove(item.transform.position + Vector3.up * 0.1f, 2).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo));

            VFX.transform.localScale *= 10;
            VFX.GetComponentInChildren<VisualEffect>().playRate = 4;

        }


        public void JumpSound()
        {
            if (GSS!=null){
                GSS.PlaySound(GenericSoundSource.GenSoundType.ItemDropping, 1, false, true);
            }
        }
    }

    
}
