using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

        public enum VFX
        {
            white,
            green,
            blue,
            purple,
            orange
        }

        private Dictionary<VFX, GameObject> VFXMap;




        public virtual void Start()
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
                return droppedItem.prefab;

                
            }

            else
            {
                return null;
            }
        }


        //Handle Spawning given a transform 
        public virtual void SpawnLoot(GameObject loot_prefab, Transform target, bool Inherit_Rotation = false, bool aschild = false)
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

            
            if (LootVFX)
            {
                SpawnVFX(LootItem.transform);
            }
        }

        //Handle Spawning given a position and a rotation 
        public virtual void SpawnLoot(GameObject loot_prefab, Vector3 pos, Quaternion rot )
        {
            


            GameObject lootitem = Instantiate(loot_prefab, pos, rot);
            //Animate Up and down movement
            lootitem.GetComponent<_Interactable>();
            if (LootVFX)
            {
                SpawnVFX(lootitem.transform);
            }
        }



        public void SpawnVFX(Transform item)
        {
            var VFX = Instantiate(GetVFXPrefab(color), item);
            item.transform.DOLocalMove(transform.position+Vector3.up*0.1f, 2).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            VFX.transform.localScale *= 10;
            VFX.GetComponentInChildren<VisualEffect>().playRate = 4;

        }
    }

    
}
