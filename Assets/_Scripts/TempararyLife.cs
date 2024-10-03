using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TempararyLife : NetworkBehaviour
    {
        public float LifeTime;
        public AudioClip[] clips;
        AudioSource AS;
        NetworkObject NO;
        private void Start()
        {
            if(TryGetComponent<AudioSource>(out AS) && clips.Length>0)
            {
                PlayRandomSound();
            }


            if (TryGetComponent<NetworkObject>(out NO))
            {
                Invoke("DespawnThingyServerRpc", LifeTime);
            }
            
        }

        public void PlayRandomSound()
        {
            AS.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }


        [ServerRpc(RequireOwnership = false)]
        public void DespawnThingyServerRpc()
        {
            Destroy(gameObject);
            NO.Despawn();
        }


    }
}
