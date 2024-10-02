using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotTag { None, Head, Chest, Legs, Feet, Weapon, Ability, Objective}
public enum Rarity { Common, Uncommon, Rare, Very_Rare, Legendary}

[Serializable]
[CreateAssetMenu(menuName = "newItem")]
public class Item : ScriptableObject
{
    public Sprite inventorySprite;
    public SlotTag itemTag;
    public Rarity rarity;
    public string itemID;


    [Header("Weapon")]
    // If SlotTag == Weapon
    public WeaponDataSO weaponData;
    [HideInInspector] public WeaponInstance weaponInstance;

    [Header("If the item can be equipped")]
    public GameObject model;
    

}
