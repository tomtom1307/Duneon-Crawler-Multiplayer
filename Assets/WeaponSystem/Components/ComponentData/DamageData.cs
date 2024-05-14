using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DamageData : ComponentData<AttackDamage>
    {
        public DamageData()
        {
            ComponentDependancy = typeof(Damage);
        }
    }
}
