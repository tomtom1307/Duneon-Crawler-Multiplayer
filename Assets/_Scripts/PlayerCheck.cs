using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class PlayerCheck : MonoBehaviour
    {
        // public List<GameObject> playersIn;
        public int playersIn = 0;
        private void OnTriggerEnter(Collider other) 
        {
            GameObject otherobject = other.gameObject;
            Debug.Log("Player entered room.");
            playersIn += 1;
/*             if (!playersIn.Contains(otherobject))
            {
                playersIn.Add(otherobject);
            } */
        }

        private void OnTriggerExit(Collider other) 
        {
            GameObject otherobject = other.gameObject;
            playersIn -= 1;
        }
    }
}
