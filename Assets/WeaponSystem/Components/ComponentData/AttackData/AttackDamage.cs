using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class AttackDamage : AttackData
    {
        [field: SerializeField] public float DamageAmount { get; private set; }
        [field: SerializeField] public float KnockBackAmount { get; private set; }

        public enum DamageType
        {
            Magic,
            Physical
        }

        [field: SerializeField] public DamageType type;
    }
}
