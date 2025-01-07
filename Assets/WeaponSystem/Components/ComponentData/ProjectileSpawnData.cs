using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class ProjectileSpawnData : ComponentData<AttackProjectileSpawn>
    {
        public int SpawnAmount;
        public bool Chargable;
        public ProjectileSpawnData()
        {
            ComponentDependancy = typeof(SpawnProjectile);
        }
    }
}
