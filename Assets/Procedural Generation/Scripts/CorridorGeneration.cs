using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class CorridorGeneration : MonoBehaviour
    {

        public Vector3 FindClosestCorridor(Vector2 Startcorridor, List<Vector3> corridors, List<int> RoomIndex)
        {
            float minDist = 100;
            Vector3 currentClosest = Vector3.zero;
            foreach (var item in corridors)
            {
                
                if (Vector2.Distance(item, Startcorridor) < minDist)
                {
                    minDist = Vector2.Distance(item, Startcorridor);
                    currentClosest = item;


                }
            }
            return currentClosest;
        }

        public int FindClosestRoom(int currentRoom, List<int> RoomIndex,List<GameObject> RoomList)
        {
            foreach(var i in RoomIndex)
            {
                //RoomList[i] = 
                return i;
            }
        }

        public void ConnectCorridors(Vector2 startPos, Vector2 endPos)
        {
            Vector2 DirVec = endPos - startPos;
            int xDir = (int)DirVec.x;
            int yDir = (int)DirVec.y;
            if(xDir >= yDir) 
            {
                //Do Pathfinding on x
            }
            else
            {
                //Do pathfinding but on other axis
            }


        }



    }
}
