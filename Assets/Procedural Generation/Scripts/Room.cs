using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Room : MonoBehaviour
    {
        public Vector2 size;
        public GameObject prefab;
        public List<Vector2> ocupied;
        public List<int> DoorPos;
        public List<Vector2> CorridorStartPos;
        public bool Considered;

        

        private void Start()
        {
            
            
        }

        public void GridClaimY()
        {

        }


    }
}
