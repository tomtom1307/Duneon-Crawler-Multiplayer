using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class AOEDamageArea : NetworkBehaviour
    {
        public float DPS;
        public float DamageTick;
        float _timer;
        PlayerStats PS;
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                PS = other.gameObject.GetComponent<PlayerStats>();
                _timer = DamageTick;   
            }
        }

        private void OnTriggerStay(Collider other)
        {
            _timer += Time.deltaTime;
            //TODO: Spawn Acid on screen VFX to indicate to the player they're 
            if(_timer > DamageTick && PS != null)
            {
                PS.TakeDamage(DPS * DamageTick, Vector3.zero, SourceSoundManager.SoundType.Hit);
                _timer = 0;
            }
            
        }
    }
}
