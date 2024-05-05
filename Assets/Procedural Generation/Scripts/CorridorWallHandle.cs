using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using static UnityEditor.Progress;

namespace Project
{
    public class CorridorWallHandle : MonoBehaviour
    {

        public enum Tiles
        {
            Empty = 0,
            RoomBound = 1,
            Corridor = 3,
            Room = 5,
            Door = 10
        }

        //Wall = 0,1
        //Empty = 3
        //Door = 10
        List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        public void TILECHECKK(List<Vector4> corridors, List<GameObject> GOs,Grid grid)
        {
            int n = 0;
            foreach (var item in corridors)
            {
                CorridorRoom go = GOs[n].GetComponent<CorridorRoom>();
                n++;
                //If dont eat shit keep it
                Vector2 pos = item;
                //Check Up

                int wallDirIndex = 0;
                foreach (var dir in directions)
                {
                    //Debug.Log(wallDirIndex);

                    int gridVal = getGridVal(pos + dir, grid);

                    if (gridVal == 3)
                    {
                        go.DoEmpty(wallDirIndex);
                    }

                    else if (gridVal == 10)
                    {
                        go.DoDoor(wallDirIndex);
                    }
                    
                    wallDirIndex++;
                }


                //Check Down

                //Check Left

                //Check Right
            }
        }

      


        public int getGridVal(Vector2 pos, Grid grid) 
        {

            int xPos = (int)pos.x;
            int yPos = (int)pos.y;
            int val = grid.GetVal(xPos,yPos);
            return val;

        }
    }
}
