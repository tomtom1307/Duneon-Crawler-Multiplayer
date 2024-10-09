using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class AbilityPageBook : _Interactable
    {

        public Canvas Canvas;

        public List<Ability> Abilities;
        public List<Ability> Equipped_Abilities;
        public List<AbilityHolder> Holders;
        UIAbilities AbilitiesUI;

        private void Start()
        {
            AbilitiesUI = GetComponent<UIAbilities>();
            Canvas = GetComponentInChildren<Canvas>();
            Canvas.enabled = false;
        }

        bool active;

        protected override void Interact()
        {
            if (active) return;
            base.Interact();
            Canvas.enabled = true;
            GetAbilities();
            foreach (var item in Abilities)
            {
                AbilitiesUI.SpawnAbilityItem(item);
            }
            foreach (var item in Equipped_Abilities)
            {
                AbilitiesUI.SpawnAbilityItem(item, true);
            }
            PlayerUI.instance.PlayerInUI(true);
            active = true;
        }

        public void GetAbilities()
        {
            Holders = new List<AbilityHolder>();
            Holders = interacter.gameObject.GetComponents<AbilityHolder>().ToList();
            AbilitiesUI.Holders = Holders;

            foreach (var item in Holders)
            {
                if (item.ability != null)
                {
                    Equipped_Abilities.Add(item.ability);
                }

            }
        }


        

        private void Update()
        {
            if (active && Input.GetKeyDown(KeyCode.Escape) && Canvas.enabled)
            {
                ResetBookInteract();
                Canvas.enabled = false;
                PlayerUI.instance.PlayerInUI(false);
            }

        }

        public void ResetBookInteract()
        {
            active = false;
            Equipped_Abilities.Clear();
            Prompt = "[F]";
        }


    }
}
