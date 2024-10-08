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

        private void Start()
        {
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
            PlayerUI.instance.PlayerInUI(true);


            active = true;
        }

        public void GetAbilities()
        {
            List<AbilityHolder> AH = new List<AbilityHolder>();
            AH = interacter.gameObject.GetComponents<AbilityHolder>().ToList();
            foreach (var item in AH)
            {
                if(item.ability != null)
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
