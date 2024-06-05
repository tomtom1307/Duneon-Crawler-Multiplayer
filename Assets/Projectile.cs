using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Projectile : MonoBehaviour
    {
        public float Speed;
        public float damage;
        public Vector3 StartPos;
        public Vector3 Direction;
        Rigidbody rb;
        TrailRenderer tr;
        // Start is called before the first frame update
        void Start()
        {
            StartPos = transform.position;
            rb = GetComponent<Rigidbody>();
            tr = GetComponent<TrailRenderer>();
            rb.AddForce(Speed * Direction, ForceMode.Impulse);
        }

        // Update is called once per frame
        void Update()
        {
            if (rb == null) return; 
           
            transform.rotation = Quaternion.LookRotation(-rb.velocity);
        }


        PlayerStats stats;
        private void OnCollisionEnter(Collision collision)
        {
            tr.enabled = false;

            transform.parent = collision.gameObject.transform;
            Destroy(rb);
            if(collision.gameObject.TryGetComponent<PlayerStats>(out stats))
            {
                stats.TakeDamage(damage, StartPos);
            }
            
            Destroy(gameObject,2f);
        }
    }
}
