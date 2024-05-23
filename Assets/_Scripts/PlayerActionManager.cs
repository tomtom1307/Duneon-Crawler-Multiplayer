using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Playables;
using UnityEngine;

namespace Project
{
    public class PlayerActionManager : NetworkBehaviour
    {
        


        public event Action<string> OnAbilityused;
        public void OnAbility(string name) => OnAbilityused?.Invoke(name);

        private void Start()
        {
            if (IsLocalPlayer)
            {
                Camera.main.GetComponentInChildren<AbilityEventHandler>().playerActionManager = this;
                Camera.main.GetComponentInChildren<AbilityEventHandler>().enabled = true;
            }
            else return;
        }


    }
}
