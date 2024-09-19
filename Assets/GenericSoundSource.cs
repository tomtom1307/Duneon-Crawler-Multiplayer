using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class GenericSoundSource : MonoBehaviour
    {
       
        public enum GenSoundType
        {
              BreakSound,
              DoorOpen
        }

        private void Start()
        {
            
        }


        public AudioSource audioSource;
        public SoundList[] clips;
        public virtual void PlaySound(GenSoundType sound, float volume, bool looping = false)
        {
            AudioClip[] _clips = clips[(int)sound].Sounds;
            AudioClip randomClip = _clips[UnityEngine.Random.Range(0, _clips.Length)];

            if (looping)
            {
                audioSource.loop = true;
                audioSource.clip = randomClip;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                audioSource.loop = false;
                audioSource.clip = null;
                audioSource.PlayOneShot(randomClip, volume);
            }


        }

        public virtual void StopSound()
        {
            audioSource.clip = null;
            audioSource.Stop();
        }


#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(GenSoundType));
            Array.Resize(ref clips, names.Length);
            for (int i = 0; i < clips.Length; i++)
            {
                clips[i].name = names[i];
            }
        }
#endif

    }

    [Serializable]
    public struct GenSoundList
    {
        public AudioClip[] Sounds { get => sounds; }
        [HideInInspector] public string name;
        [SerializeField] public AudioClip[] sounds;
    }
    
}
