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
            Debug.Log("PlayerSoundSource Awake called");

            Debug.Log("Instance is:", Instance);
            if (Instance == null)
            {
                Instance = this;
                Debug.Log("PlayerSoundSource Instance set");
            }
            else
            {
                Destroy(gameObject);
                Debug.Log("Duplicate PlayerSoundSource destroyed");
            }
        }
    }





}

