using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class SecondaryAttackDamage : AttackData
    {
        [field: SerializeField] public float DamageAmount { get; private set; }
        [field: SerializeField] public float KnockBackAmount { get; private set; }
    }
}
