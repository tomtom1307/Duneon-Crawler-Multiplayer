using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Teleport : MonoBehaviour
    {
        public Transform TeleportPosition;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                other.gameObject.transform.position = TeleportPosition.position;
            }
        }
    }
}
