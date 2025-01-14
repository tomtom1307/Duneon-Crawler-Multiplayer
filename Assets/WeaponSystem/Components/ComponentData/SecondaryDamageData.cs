using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class SecondaryDamageData : ComponentData<SecondaryAttackDamage>
    {
        public bool HitNoneEnemies = true;
        public bool Chargable = false;
        
        public SecondaryDamageData()
        {
            
            ComponentDependancy = typeof(SphereOverlapDamage);
        }
    }
}
