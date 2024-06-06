using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Project
{
    public class PlayerSpawner : MonoBehaviour
    {   
        public List<GameObject> players;
        public List<Transform> SpawnPositions;
        // Start is called before the first frame update
        private void Awake() 
        {
            players = GameObject.FindGameObjectsWithTag("Player").ToList();
            for(int i=0; i< players.Count; i++)
            {
                players[i].transform.position = SpawnPositions[i].position;
            }
        }
    }
}
