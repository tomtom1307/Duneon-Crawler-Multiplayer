using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Footsteps : MonoBehaviour
    {
        PlayerMovement PM;
        float timer;
        public float timeBetweenSteps;
        public AnimationCurve VelocityDependance;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (PM == null)
            {
                GameObject[] GOs = GameObject.FindGameObjectsWithTag("Player");
                foreach (var item in GOs)
                {
                    item.TryGetComponent<PlayerMovement>(out PM);
                    if (PM != null) break;
                }
                return;
            }

            if (!PM.grounded || PM.rb.velocity.magnitude < 0.7f)
            {
                return;
            }
            timeBetweenSteps = VelocityDependance.Evaluate(PM.rb.velocity.magnitude);
            timer += Time.deltaTime;
            if (timer > timeBetweenSteps)
            {
                timer = 0;
                PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.FootStep, 0.5f);
            }
            

            
        }
    }
}
