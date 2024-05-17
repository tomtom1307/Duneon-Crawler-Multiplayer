using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class InventoryItem : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;
    [SerializeField] public Item myItem;
    public CanvasGroup canvasGroup {  get; private set; }

    
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
        itemIcon.sprite = item.sprite;
        itemIcon.color = rarity;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Singleton.SetCarriedItem(this);
        }
    }

    
}
