using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotTag { None, Head, Chest, Legs, Feet, Weapon, Ability}
public enum Rarity { Common, Uncommon, Rare, Very_Rare, Legendary}

public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;
    public Rarity rarity;
    public WeaponDataSO weaponData;
    public WeaponInstance weaponInstance;

    [Header("If the item can be equipped")]
    public GameObject prefab;
    

}
