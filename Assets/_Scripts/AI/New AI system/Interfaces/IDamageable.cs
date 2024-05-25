using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public interface IDamageable
    {

        void Damage(float DamageAmount);

        void Die();

        float MaxHealth {  get; set; }
        float CurrentHealth { get; set; }
        
    }
}
