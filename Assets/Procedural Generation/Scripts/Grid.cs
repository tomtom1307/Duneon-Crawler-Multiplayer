using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

namespace Project
{
    public class Grid
    {
        public int width;
        public int height;
        private float cellSize;
        int[,] gridArray;
        private TextMesh[,] textvalues;
        public List<Vector2> bounds;

        public Grid(int width, int height, float cellSize)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            

            //+2 to add boundary around the grid to catch edge cases
            gridArray = new int[width, height];
            textvalues = new TextMesh[width, height];
            bounds = new List<Vector2>();
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    textvalues[x,y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPos(x, y)+new Vector3(cellSize,0,cellSize)/2, 20, Color.white, TextAnchor.MiddleCenter);
                    textvalues[x,y].transform.rotation = Quaternion.Euler(90, 0, 0);
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 100f);
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y+1), Color.white, 100f);
                    if(x == 0 || y == 0 || x == width-1|| y == height-1)
                    {
                        SetVal(x, y, 5);
                        bounds.Add(new Vector2(x, y));
                    }
                    
                }
            }
            Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 100f);


            
        }

        public Vector3 GetWorldPos(int x, int y)
        {
            return new Vector3(x, 0, y) * cellSize;
        }


        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x/cellSize);
            y = Mathf.FloorToInt(worldPosition.z/cellSize);
        }

        public void SetVal(int x, int y, int value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                textvalues[x, y].text = gridArray[x, y].ToString();
            }
        }


        public void SetVal(Vector3 worldPos, int value)
        {
            int x, y;
            GetXY(worldPos, out x, out y);
            SetVal(x,y, value);
        }

        public int GetVal(int x, int y)
        {
            string val = textvalues[x, y].text;
            return int.Parse(val);
        }



       

    }
}
