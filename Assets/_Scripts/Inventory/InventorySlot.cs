using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InventoryItem myItem { get;  set; }
    public int SlotVal;
    public SlotTag myTag;



    public void SetItem(InventoryItem item, bool Swap = false)
    {
        Inventory.carriedItem = null;

        item.activeSlot.myItem = null;


        myItem = item;
        myItem.activeSlot = this;
        myItem.transform.SetParent(transform);
        myItem.transform.localPosition = Vector3.zero;
        myItem.canvasGroup.blocksRaycasts = true;

        if(myTag != SlotTag.None)
        {
            Inventory.Singleton.EquipEquipment(myTag, myItem, Swap, SlotVal);
        }


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("SlotMouseUp");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inventory.carriedItem == null) return;
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag)
            {
                Inventory.carriedItem.activeSlot.SetItem(Inventory.carriedItem);
                return;
            }
            SetItem(Inventory.carriedItem);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Inventory.carriedItem == null) return;
            if (myTag != SlotTag.None && Inventory.carriedItem.myItem.itemTag != myTag)
            {
                Inventory.carriedItem.activeSlot.SetItem(Inventory.carriedItem);
                return;
            }
            SetItem(Inventory.carriedItem);
        }
    }
}
