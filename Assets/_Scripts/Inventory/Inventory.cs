using Project;
using Project.Assets.WeaponSystem;
using Project.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Singleton;
    public static InventoryItem carriedItem;

    [SerializeField] InventorySlot[] inventorySlots;

    [SerializeField] Transform draggablesTransform;
    [SerializeField] InventoryItem itemPrefab;

    GameObject GWL;
    GameObject GWM;
    [Header("Item List")]
    [SerializeField] Item[] items;
    [SerializeField] HandStuff handLoc;
    [SerializeField] Transform ClientWeaponTransform;
    [SerializeField] HandMult handMult;
    [SerializeField] WeaponHolder WH;
    [SerializeField] WeaponGenerator WG;
    PlayerAttack PA;

    private void Awake()
    {
        
        Singleton = this;
        handLoc = FindAnyObjectByType<HandStuff>();
        handMult = FindAnyObjectByType<HandMult>();
        PA = handLoc.gameObject.GetComponent<PlayerAttack>(); 
        ClientWeaponTransform = handLoc.transform.Find("Base");
        //WH = FindAnyObjectByType<WeaponHolder>();
        
    }

    private void Update()
    {
        //if (handMult == null) handMult = FindObjectOfType<HandMult>();
        if (carriedItem == null) return;
        
        
        carriedItem.transform.position = Input.mousePosition;

    }

    public void SetCarriedItem(InventoryItem item)
    {
        if(carriedItem != null)
        {
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag) return;
            item.activeSlot.SetItem(carriedItem);
        }

        if (item.activeSlot.myTag != SlotTag.None)
        { EquipEquipment(item.activeSlot.myTag, null); }
            
        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        item.transform.SetParent(draggablesTransform);
        
    }

    public bool Active;


    public void EquipEquipment(SlotTag tag, InventoryItem item = null)
    {
        
        switch (tag)
        {
            case SlotTag.Weapon:
                
                if(item ==null)
                {
                    Active = false;
                    WH.enabled = false;
                    PA.enabled = false;
                    WG.RemoveWeapon();

                    
                    //Destroy(GWL);
                }
                else
                {
                    Active = true;
                    
                    PA.enabled = true;
                    WH.enabled = true;
                    WG.GenerateWeapon(item.myItem.weaponData);
                    WH.statManager.weaponInstance = item.myItem.weaponInstance; 

                }
                break;
            case SlotTag.Head:
                if(item == null)
                {
                    Debug.Log("Unequipped helmet on " + tag);
                }
                else
                {
                    Debug.Log("Equipped" + item.myItem.name + "on" + tag);
                }
                break;
            case SlotTag.Chest:
                if (item == null)
                {
                    Debug.Log("Unequipped Chest on " + tag);
                }
                else
                {
                    Debug.Log("Equipped" + item.myItem.name + "on" + tag);
                }
                break;
            case SlotTag.Legs:
                if (item == null)
                {
                    Debug.Log("Unequipped Legs on " + tag);
                }
                else
                {
                    Debug.Log("Equipped" + item.myItem.name + "on" + tag);
                }
                break;
            case SlotTag.Feet:
                if (item == null)
                {
                    Debug.Log("Unequipped Feet on " + tag);
                }
                else
                {
                    Debug.Log("Equipped" + item.myItem.name + "on" + tag);
                }
                break;
        }
    }

    public void SpawnInventoryItem(Color rarity, Item item = null)
    {
        Item _item = item;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Check if the slot is empty
            if (inventorySlots[i].myItem == null)
            {
                Instantiate(itemPrefab, inventorySlots[i].transform).Initialize(_item, inventorySlots[i],rarity);
                break;
            }
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }


}
