using DG.Tweening;
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
        public Transform IKTarget;
        public bool SetIk;
        public string[] abilityNames;
        public Vector3 IKpos;
        private void Start()
        {
            anim = GetComponent<Animator>();
            SetIk = true;
            
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


        //Call 
        public void SetIKToOrigin()
        {
            IKTarget.DOLocalMove(IKpos,.3f);
        }


        private void Update()
        {
            if (SetIk)
            {
                SetIKToOrigin();
            }
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
