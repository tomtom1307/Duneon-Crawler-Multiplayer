using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class Vase : MonoBehaviour
    {
        [SerializeField] GameObject BrokenVase;
        DamageableThing DT;
        public float BreakForce;

        private void Start()
        {
            DT = GetComponent<DamageableThing>();
        }

        private void Update()
        {
            if (DT.ded)
            {
                Break();
            }
        }

        private void Break()
        {
            var broken = Instantiate(BrokenVase,transform.position,transform.rotation);
            Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
                rb.AddExplosionForce(BreakForce, transform.position, 1);
                Destroy(rb.gameObject, 2);
                Destroy(rb.transform.parent.gameObject, 2.1f);
            }


            
            this.gameObject.SetActive(false);
        }
    }
}
