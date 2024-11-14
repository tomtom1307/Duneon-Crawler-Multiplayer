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
        public GameObject playerPrefab;
        public List<Transform> SpawnPositions;
        // Start is called before the first frame update

        private void OnEnable()
        {
            //Actions.PlayerStart += PlayerSpawn; //Listening to action of Player starting game
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerObject;
        }
        private void OnDisable() 
        {
            //Actions.PlayerStart -= PlayerSpawn;
        }
        private void PlayerSpawn() 
        {
            Debug.Log("PlayerSpawn() called.");
            players = GameObject.FindGameObjectsWithTag("Player").ToList(); // Finds all player objects
            int i = 0;
            foreach(GameObject player in players)
            {
                player.transform.position = SpawnPositions[i].position; // Moves all players to the scene spawn positions
                Debug.Log("PlayerSpawn i:"+ i);
                i++;
            }
        }

        public Vector3 GetAvailablePosition()
        {
            return SpawnPositions[0].position;
        }

        private void SpawnPlayerObject(ulong joinedClientId)
        {
            Debug.Log("SpawnPlayerObj");
            GameObject newPlayer = Instantiate(playerPrefab, GetAvailablePosition(), Quaternion.identity);
            NetworkObject NO = newPlayer.GetComponent<NetworkObject>();
            NO.SpawnAsPlayerObject(joinedClientId);

        }
    }
}
