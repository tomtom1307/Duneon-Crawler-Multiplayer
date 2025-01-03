using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode.Components;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class PlayerProjectile : NetworkBehaviour
    {
        public GameObject SpawnOnDeath;
        public bool ParentOnHit;
        public float Speed;
        public float damage;
        public Vector3 StartPos;
        public Vector3 Direction;
        Rigidbody rb;
        NetworkRigidbody Nrb;
        NetworkObject NO;
        bool hit;
        // Start is called before the first frame update
        void Start()
        {
            StartPos = transform.position;
            rb = GetComponent<Rigidbody>();
            Nrb = GetComponent<NetworkRigidbody>();
            hit = false;
            rb.isKinematic = false;
            rb.AddForce(Speed * Direction, ForceMode.Impulse);
        }

        // Update is called once per frame
        void Update()
        {
            if (rb == null) return;

            if (rb.velocity.magnitude > 1f)
            {
                transform.rotation = Quaternion.LookRotation(-rb.velocity);
            }

        }


        Enemy enemy;
        DamageableThing TD;
        Quaternion rot;
        private void OnCollisionEnter(Collision collision)
        {
            OnCollision(collision);
        }

        public virtual void OnCollision(Collision collision)
        {
            if (collision.collider.gameObject.layer == 10) return;
            if (hit) return;
            
            rot = transform.rotation;
            
            hit = true;

            Destroy(Nrb);
            Destroy(rb);
            GetComponent<Collider>().enabled = false;
            if (collision.gameObject.TryGetComponent<Enemy>(out enemy))
            {
                if (ParentOnHit)
                {
                    transform.parent = collision.gameObject.transform;
                    transform.rotation = rot;
                }
                
                enemy.DoDamageServerRpc(damage);
                enemy.KnockBackServerRpc(StartPos, 10f);
                DestroyProjectile();
                
            }
            
            else if(collision.gameObject.TryGetComponent(out TD))
            {
                TD.TakeDamageServerRpc(damage);
                DestroyProjectile();
            }
            
            else if (IsOwner)
            {
                DestroyProjectile();
            }
            
            
        }

        public void DestroyProjectile()
        {
            if (SpawnOnDeath != null)
            {
                //Spawn thing 
                var bum = Instantiate(SpawnOnDeath, transform.position, Quaternion.identity);
                bum.GetComponent<NetworkObject>().Spawn();
            }
            if (IsOwner) Destroy(gameObject);
        }
    }
}
