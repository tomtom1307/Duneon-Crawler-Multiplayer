using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class CorridorGeneration : MonoBehaviour
    {

        
        public Vector4 FindClosestCorridor(int CurrentRoom, List<Vector4> corridors, List<int> AvailableRooms, out int ChosenRoom, out Vector4 startCoordior)
        {
            float minDist = 100;
            Vector4 currentClosestEnd = Vector4.zero;
            Vector4 currentClosestStart = Vector4.zero;
            List<Vector4> PossiblestartPos = ChooseRoomCorridor(CurrentRoom, corridors);
            
            
            //Choose a corridor given a room number based on the boolean in the w component

            foreach (Vector4 start in PossiblestartPos)
            {
                foreach (var item in corridors)
                {
                    if (Vector2.Distance(item, start) < minDist && AvailableRooms.Contains((int)item.z))
                    {
                        currentClosestStart = start;
                        minDist = Vector2.Distance(item, start);
                        currentClosestEnd = item;
                    }
                }
            }
            ChosenRoom = (int)currentClosestEnd.z;
            startCoordior = currentClosestStart;
            currentClosestStart.w = 1;
            currentClosestEnd.w = 1;
            return currentClosestEnd;
        }

        public List<Vector4> ChooseRoomCorridor(int CurrentRoom, List<Vector4>Corridors)
        {
            //Initialize a list of all available corridors corresponding to the current room
            List<Vector4>availableCorridorsConsidered = new List<Vector4>();
            List<Vector4>availableCorridors = new List<Vector4>();

            //Loop through all corridors to find corridors belonging to the current room
            foreach (var item in Corridors)
            {

                if ((int)item.z == CurrentRoom)
                {
                    availableCorridors.Add(item);
                }
            }
            //Once all the room corridors are found 
            foreach (var item in availableCorridors)
            {
                //Check if any of the corridors are have not made any connections
                if(item.w != 0)
                {
                    availableCorridorsConsidered.Add(item);
                    availableCorridors.Remove(item);
                    
                }

            }
            //If this fails return a random one
            if(availableCorridors.Count > 0)
            {
                return availableCorridors;
            }
            else
            {
                return availableCorridorsConsidered;
            }
        }


        public int FindClosestRoom(int currentRoom, List<int> RoomIndex,List<GameObject> RoomList)
        {
            foreach(var i in RoomIndex)
            {
                //RoomList[i] = 
                return i;
            }
            return RoomList.Count;

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
