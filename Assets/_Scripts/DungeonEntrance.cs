using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DungeonEntrance : MonoBehaviour
    {
        public Vector3 startRoomPos = Vector3.zero;
        public float spawnRange = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 10)
            {
                GameObject GO = GameObject.FindGameObjectsWithTag("StartRoom")[0];
                startRoomPos = GO.transform.position;
                other.gameObject.transform.position = startRoomPos + Vector3.up + Random.Range(-spawnRange, spawnRange) * Vector3.right + Random.Range(-spawnRange, spawnRange) * Vector3.forward;
            }

        }

    }
}
