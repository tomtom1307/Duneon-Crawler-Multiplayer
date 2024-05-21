using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

namespace Project
{

    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public abstract class SourceSoundManager : NetworkBehaviour
    {
        public enum SoundType
        {
            Attack1,
            Attack2Charge,
            Attack2Charged,
            Attack2Release,
            HeadShotDink,
            Hit,
            Jump
        }


        public AudioSource audioSource;
        public SoundList[] clips;
        public virtual void PlaySound(SoundType sound, float volume)
        {
            AudioClip[] _clips = clips[(int)sound].Sounds;
            AudioClip randomClip = _clips[UnityEngine.Random.Range(0, _clips.Length)];


            audioSource.PlayOneShot(randomClip, volume);
        }

        public virtual void StopSound() 
        {
            audioSource.Stop();
        }


#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref clips, names.Length);
            for (int i = 0; i < clips.Length; i++) {
                clips[i].name = names[i];
            }
        }
#endif

    }

    [Serializable]
    public struct SoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [HideInInspector] public string name;
        [SerializeField] public AudioClip[] sounds;
    }


}
