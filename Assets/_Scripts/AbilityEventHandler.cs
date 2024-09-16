using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class AbilityEventHandler : MonoBehaviour
    {
        public Animator anim;
        public PlayerActionManager playerActionManager;
        public string[] abilityNames;

        private void Start()
        {
            anim = GetComponent<Animator>();
           
        }

        public void StartAnimation(string AnimationName)
        {
            foreach (string abilityName in abilityNames)
            {
                if (abilityName == AnimationName)
                {
                    anim.Play(AnimationName);
                    
                }
                
            }
            return;
            
        }


        public void CamShakeGoesBRRR()
        {
            CamShake cs = Camera.main.GetComponent<CamShake>();
            cs.StartShake(cs.onAOE);
        }

        public void PlayerSound(SourceSoundManager.SoundType soundType)
        {
            PlayerSoundSource.Instance.PlaySound(soundType, 1);
        }

        private void OnEnable()
        {
            playerActionManager.OnAbilityused += StartAnimation;
        }


        private void OnDisable()
        {
            playerActionManager.OnAbilityused -= StartAnimation;
        }



    }
}
