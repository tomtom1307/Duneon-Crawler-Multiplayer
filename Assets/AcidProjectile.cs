using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

namespace Project
{
    public class AcidProjectile : Projectile
    {
        public GameObject OnLandPuddle;

        private void Update()
        {
            
        }
        private void OnCollisionEnter(Collision collision)
        {
            //OnCollision(collision);
            if(collision.gameObject.layer == 6)
            {
                GameObject GO = Instantiate(OnLandPuddle, transform.position, Quaternion.identity);
                GO.GetComponent<NetworkObject>().Spawn();
                gameObject.GetComponent<VisualEffect>().SetBool("CenterblobsOnOff",false);
                gameObject.GetComponent<SphereCollider>().enabled = false;
                Destroy(gameObject,2);
            }
            else if ( collision.gameObject.layer == 10)
            {
                collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage, Vector3.zero, SourceSoundManager.SoundType.Hit);
                GameObject GO = Instantiate(OnLandPuddle, transform.position, Quaternion.identity);
                GO.GetComponent<NetworkObject>().Spawn();
                gameObject.GetComponent<VisualEffect>().SetBool("CenterblobsOnOff", false);
                gameObject.GetComponent<SphereCollider>().enabled = false;
                Destroy(gameObject,2);
            }
        }
    }
}
