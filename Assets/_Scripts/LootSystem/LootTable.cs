using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "LootTable", menuName = "Loot/LootTable")]
    public class LootTable : ScriptableObject
    {
        public List<LootItem> List;

        

        public GameObject whiteVFXPrefab;
        public GameObject greenVFXPrefab;
        public GameObject blueVFXPrefab;
        public GameObject purpleVFXPrefab;
        public GameObject orangeVFXPrefab;




    }

    [Serializable]
    public struct LootItem
    {
        //The data object to store the loot items
        //Might need to refactor this with the current "Item" System so that weapons can be included not sure tho
        public string Name;
        //Color of the VFX if required
        public LootGenerator.VFX VFXColor;
        //Value from 0 to 100 
        public float dropChance;
        public GameObject prefab;




    }
}
