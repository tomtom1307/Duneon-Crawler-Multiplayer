using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;


namespace Project
{
    public class PlayerSpawner : NetworkBehaviour
    {   
        public List<GameObject> players;
        public List<Transform> SpawnPositions;
        // Start is called before the first frame update

        private void OnEnable()
        {
            Actions.PlayerStart += PlayerSpawn; //Listening to action of Player starting game
        }
        private void OnDisable() 
        {
            Actions.PlayerStart -= PlayerSpawn;
        }
        private void PlayerSpawn() 
        {
            Debug.Log("PlayerSpawn() called.");
            players = GameObject.FindGameObjectsWithTag("Player").ToList(); // Finds all player objects
            for(int i=0; i< players.Count; i++)
            {
                players[i].transform.position = SpawnPositions[i].position; // Moves all players to the scene spawn positions
            }
        }
    }
}
