using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class SphereOverlapData : ComponentData<AttackOverlapSphere>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }
        [field: SerializeField] public float detectionRadius { get; private set; }

        [field: SerializeField] public Vector3 offset { get; private set; }

        private Camera cam;
        
        public bool Chargable;
        public SphereOverlapData()
        {
            ComponentDependancy = typeof(ActionSphereOverlap);
        }
    }
}
