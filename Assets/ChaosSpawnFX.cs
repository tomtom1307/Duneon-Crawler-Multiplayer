using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaosSpawnFX : MonoBehaviour
    {

        public LayerMask ground;
        private void Start()
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, 10, ground))
            {
                transform.position = hit.point;
            }
        }
    }
}
