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
    public List<InventorySlot> WeaponSlots;


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
        Active = false;
        Singleton = this;

        //Get reference to hand object so getting necessary components is easier 
        handLoc = FindAnyObjectByType<HandStuff>();

        //Get the local hand model
        GWL = handLoc.GetComponentInChildren<AbilityEventHandler>().gameObject;

        //Multiplayer Hand Model
        handMult = FindAnyObjectByType<HandMult>();

        //Get Player Attack input 
        PA = handLoc.gameObject.GetComponent<PlayerAttack>(); 

        //Position of weapons
        ClientWeaponTransform = handLoc.transform.Find("Base");


        //WH = FindAnyObjectByType<WeaponHolder>();
        
    }

    private void Update()
    {
        //if (handMult == null) handMult = FindObjectOfType<HandMult>();

        //If not moving an item in inventory do nothing 
        if (carriedItem == null) return;
        
        //if moving something set that moved thing to the mouse position
        carriedItem.transform.position = Input.mousePosition;

    }


    //Called on Click to pick up an item
    public void SetCarriedItem(InventoryItem item, bool swap  = false)
    {
        //If carrying an item
        if(carriedItem != null)
        {
            //and the slot currently the mouse is hovering above contains an item as well but the current carried item is a mismatch in tags return
            if (item.activeSlot.myTag != SlotTag.None && item.activeSlot.myTag != carriedItem.myItem.itemTag)
            {
                return;
            }


            //otherwise swap the itms
            item.activeSlot.SetItem(carriedItem, true);
        }




        else if (item.activeSlot.myTag != SlotTag.None)
        {
            EquipEquipment(item.activeSlot.myTag, null);
        }
            


        carriedItem = item;
        carriedItem.canvasGroup.blocksRaycasts = false;
        carriedItem.canvasGroup.interactable = false;
        item.transform.SetParent(draggablesTransform);
        
    }



    //Want to impliment an autoequip feature if the slot that this item occupies is already empty 
    public void AutoEquip()
    {
        
    }

    public bool Active;


    //Called to equip items such as armor or weapons
    public void EquipEquipment(SlotTag tag, InventoryItem item = null, bool swap = false)
    {
        
        switch (tag)
        {
            case SlotTag.Weapon:
                
                //Dequip
                if(item ==null)
                {
                    Active = false;
                    GWL.SetActive(false);
                    WH.enabled = false;
                    PA.enabled = false;
                    WG.RemoveWeapon();

                    
                    //Destroy(GWL);
                }

                //Swap the items
                else if (swap)
                {
                    Active = false;
                    WH.enabled = false;
                    WG.SwapWeapon(item.myItem.weaponInstance);
                    Active = true;
                    WH.enabled = true;
                }
                
                //Equip
                else
                {
                    Active = true;
                    GWL.SetActive(true);
                    if (!PA.enabled)
                    {
                        PA.enabled = false;
                    }
                    WH.enabled = true;
                    WG.GenerateWeapon(item.myItem.weaponInstance);

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
