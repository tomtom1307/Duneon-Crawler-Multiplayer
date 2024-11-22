using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DirectionalLightController : MonoBehaviour
    {
        public List<GameObject> lightsToDisable;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (GameObject go in lightsToDisable)
            {
                go.SetActive(false);
            }
        }
    }
}
