using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public interface IDamageable
    {

        void DoDamageServerRpc(float DamageAmount, bool headshot = false);

        float MaxHealth {  get; set; }

        NetworkVariable<float> CurrentHealth { get; set; }

        DissolveController dissolve { get; set; }

        int xpOnKill { get; set; }

        // VFX Damage effects
        //Text Vars
        public Color HeadshotColor { get; set; }
        public Color NormalColor { get; set; }

        //Flash Vars

        Material[] origColors { get; set; }
        Material[] whites { get; set; }

        float flashTime { get; set; }


    }
}
