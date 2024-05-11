using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class PlayerCheck : MonoBehaviour
    {
        public List<GameObject> playersIn;
        public int PlayersNeeded;


        private void OnTriggerEnter(Collider other) 
        {
            GameObject otherobject = other.gameObject;
            Debug.Log("Player entered room.");
            if (!playersIn.Contains(otherobject))
            {
                playersIn.Add(otherobject);
            }
            if (playersIn.Count == PlayersNeeded)
            {
                Debug.Log("Room occupation condition satisfied.");
                //Close room and start interactions
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            GameObject otherobject = other.gameObject;
            playersIn.Remove(otherobject);
        }
    }
}
