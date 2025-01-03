using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class GenericSoundSource : MonoBehaviour
    {
       // ADD NEW SOUND TYPES HERE FOR ARRAY TO CHANGE IN INSPECTOR
       //Please add new sounds to the bottom to avoid offsetting older sounds Im sorry this is kind of jank.
        public enum GenSoundType
        {
              BreakSound,
              DoorOpen,
              Elevator,
              ElevatorStop,
              KeySlotting,
              ItemDropping,
              
        }

        public float StartingPitch;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            StartingPitch = audioSource.pitch;
        }




        [HideInInspector] public AudioSource audioSource;
        public SoundList[] clips;
        public virtual void PlaySound(GenSoundType sound, float volume, bool looping = false, bool varyPitch = false)
        {
            AudioClip[] _clips = clips[(int)sound].Sounds;
            if (_clips.Length == 0) return;
            AudioClip randomClip = _clips[UnityEngine.Random.Range(0, _clips.Length)];

            if (varyPitch)
            {
                audioSource.pitch = UnityEngine.Random.Range(StartingPitch-0.2f, StartingPitch + 0.2f);
            }
            else
            {
                audioSource.pitch = StartingPitch;
            }

            if (looping)
            {
                
                audioSource.loop = true;
                audioSource.clip = randomClip;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                audioSource.volume = volume;
                audioSource.loop = false;
                audioSource.clip = null;
                audioSource.PlayOneShot(randomClip, volume);
            }


        }

        public void FadeSound(float stopVol,float timeBetween)
        {
            float startVol = audioSource.volume;
            DOVirtual.Float(startVol, stopVol, timeBetween, val =>
            {

                audioSource.volume = val;
            });
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
