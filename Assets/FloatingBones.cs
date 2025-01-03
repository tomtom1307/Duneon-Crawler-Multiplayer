using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class FloatingBones : MonoBehaviour
    {
        public float FloatForce;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Bones")
            {
                other.GetComponent<Rigidbody>().velocity *= 0.5f;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "Bones")
            {
                other.GetComponent<Rigidbody>().AddForce(FloatForce* Vector3.up);
            }
        }
    }
}
