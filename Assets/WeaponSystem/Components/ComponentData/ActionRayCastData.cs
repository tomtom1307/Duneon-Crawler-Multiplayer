using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ActionRayCastData : ComponentData<AttackActionRayCast>
    {
        [field: SerializeField] public LayerMask DetectableLayers{ get; private set; }
        [field: SerializeField] public float detectionRadius{ get; private set; }
        private Camera cam;  

        public ActionRayCastData()
        {
            ComponentDependancy = typeof(ActionRayCast);
        }

    }
}
