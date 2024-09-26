using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public Image itemIcon;
    [SerializeField] public Item myItem;
    public CanvasGroup canvasGroup {  get; private set; }


    public SlotTag myTag;
    
    public InventorySlot activeSlot { get; set; }

    
    void Awake()
    {
        
    }

    public void Initialize(Item item, InventorySlot parent, Color rarity)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemIcon = GetComponent<Image>();
        activeSlot = parent;
        activeSlot.myItem = this;
        myItem = item;
        itemIcon.sprite = item.inventorySprite;
        itemIcon.color = rarity;
        myTag = item.itemTag;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Held Button");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("shiftClick");
                InventorySlot Slot = Inventory.Singleton.WeaponSlots.FirstOrDefault(n => n.myItem == null);
                if(Slot != null)
                {
                    Slot.SetItem(this);
                    
                }
                return;

            }
            Inventory.Singleton.SetCarriedItem(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Released Button");
            if (Inventory.carriedItem == null) return;
            activeSlot.SetItem(this);


        }
        */

    }
}
