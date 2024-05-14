using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class SecondaryDamageData : ComponentData<SecondaryAttackDamage>
    {
        public SecondaryDamageData()
        {
            ComponentDependancy = typeof(SecondaryAttack);
        }
    }
}
