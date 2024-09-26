using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotTag { None, Head, Chest, Legs, Feet, Weapon, Ability}
public enum Rarity { Common, Uncommon, Rare, Very_Rare, Legendary}

public class Item : ScriptableObject
{
    public Sprite inventorySprite;
    public SlotTag itemTag;
    public Rarity rarity;


    [Header("Weapon")]
    // If SlotTag == Weapon
    public WeaponDataSO weaponData;
    [HideInInspector] public WeaponInstance weaponInstance;

    [Header("If the item can be equipped")]
    public GameObject model;
    

}
