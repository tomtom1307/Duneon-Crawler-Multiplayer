using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Project
{
    public class Projectile : NetworkBehaviour
    {
        public float Speed;
        public float damage;
        public Vector3 StartPos;
        public Vector3 Direction;
        Rigidbody rb;
        NetworkRigidbody Nrb;
        NetworkObject NO;
        bool hit;
        TrailRenderer tr;
        // Start is called before the first frame update
        void Start()
        {
            StartPos = transform.position;
            rb = GetComponent<Rigidbody>();
            Nrb = GetComponent<NetworkRigidbody>();
            hit = false;
            tr = GetComponent<TrailRenderer>();
            rb.isKinematic = false;
            rb.AddForce(Speed * Direction, ForceMode.Impulse);
        }

        // Update is called once per frame
        void Update()
        {
            if (rb == null) return; 
           
            if(rb.velocity.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(-rb.velocity);
            }
            
        }


        PlayerStats stats;
        private void OnCollisionEnter(Collision collision)
        {
            if (hit)return;
            
            tr.enabled = false;
            hit = true;
            
            Destroy(Nrb);
            Destroy(rb);
            GetComponent<Collider>().enabled = false;
            if (collision.gameObject.TryGetComponent<PlayerStats>(out stats))
            {
                transform.parent = collision.gameObject.transform;
                stats.TakeDamage(damage, StartPos);
            }
            
            if(IsOwner) Destroy(gameObject, 2f);
        }
    }
}
