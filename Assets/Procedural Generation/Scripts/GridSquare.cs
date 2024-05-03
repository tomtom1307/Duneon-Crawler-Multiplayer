using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GridSquare : MonoBehaviour
    {
        public Vector2 position;

        Material regular;
        public Material StartColor;
        public Material EndColor;

        public bool StartPos;
        public bool EndPos;
        public bool Room;

        private void Start()
        {
            
        }


        private void ChangeColor(Material mat)
        {
            this.gameObject.GetComponent<MeshRenderer>().material = mat;
        }

        private void Update()
        {
            
        }
    }
}
