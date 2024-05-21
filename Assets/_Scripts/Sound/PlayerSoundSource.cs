using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Project
{
    public class PlayerSoundSource : SourceSoundManager
    {
        public static PlayerSoundSource Instance;

        private void Awake()
        {
            if (!IsOwner)
            {
                return;
            }
            Instance = this;
        }


        

        

    }
}
