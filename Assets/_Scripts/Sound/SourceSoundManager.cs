using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{

    [RequireComponent(typeof(AudioSource))]
    public abstract class SourceSoundManager : NetworkBehaviour
    {
        public AudioSource audioSource;
        public AudioClip[] clip;
        public virtual void PlaySound(AudioClip clip, float volume)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
