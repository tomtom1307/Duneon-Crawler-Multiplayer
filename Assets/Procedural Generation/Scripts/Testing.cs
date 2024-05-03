using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Diagnostics;

namespace Project
{
    
    public class Testing : MonoBehaviour
    {
        public float CellSize;
        public int width;
        public int height;
        public Grid grid;
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void GenerateGrid()
        {
            grid = new Grid(width, height, CellSize);
        }

        
    }
}
