using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class UIAbilityItem : MonoBehaviour, IPointerDownHandler
    {
        public Ability myAbility;
        public Image sprite;
        public CanvasGroup canvasGroup;

        public UIAbilitySlot activeSlot { get; set; }

        public void Initialize(Ability item, UIAbilitySlot parent)
        {
            myAbility = item;
            activeSlot = parent;
            activeSlot.myItem = this;
            sprite.sprite = myAbility.InventorySprite;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

    }
}
