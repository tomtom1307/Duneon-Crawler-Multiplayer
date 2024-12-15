using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class AttackProjectileSpawn : AttackData
    {
        public GameObject Projectile;
        public float Deviation;
        public float Damage;
        public float Speed;

    }
}
