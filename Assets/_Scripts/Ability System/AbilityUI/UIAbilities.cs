using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class UIAbilities : MonoBehaviour
    {
        [SerializeField] List<UIAbilitySlot> Slots = new List<UIAbilitySlot>();
        [SerializeField] List<UIAbilitySlot> EquippedSlots = new List<UIAbilitySlot>();
        public static UIAbilityItem carriedItem;
        public Transform draggablesTransform;
        public UIAbilityItem itemPrefab;

        public List<Ability> Allabilities;
        public List<Ability> EquippedAbilities;
        public List<AbilityHolder> Holders;
        public static UIAbilities instance;
        private void Start()
        {
            if(instance == null)
            {
                instance = this;
            }
        }


        public bool AlreadyExists(Ability item, List<Ability> slots)
        {
            if (slots.Contains(item))
            {
                return true;
            }

            return false;
        }

        public void DequipAbilityItem()
        {

        }



        public void SpawnAbilityItem(Ability item, bool Equipped = false)
        {
            
            UIAbilityItem SpawnedabilityItem = null;
            Ability ability = item;
            List<UIAbilitySlot> slots = null;
            if (Equipped)
            {
                if(AlreadyExists(item, EquippedAbilities))
                {
                    return;
                }
                EquippedAbilities.Add(item);
                slots = EquippedSlots;
            }
            else
            {
                if (AlreadyExists(item, Allabilities)) return;
                Allabilities.Add(item);
                slots = Slots;
            }
            foreach (var slot in slots)
            {
                if (slot.myItem == null)
                {
                    SpawnedabilityItem = Instantiate(itemPrefab, slot.transform);

                    SpawnedabilityItem.Initialize(ability, slot);
                    break;
                }
            }

        }




    }
}
