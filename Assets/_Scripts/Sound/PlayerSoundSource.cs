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


        private enum SoundType
        {
            Attack1,
            Attack2,
            Hit,
            Jump
        }

        

    }
}
