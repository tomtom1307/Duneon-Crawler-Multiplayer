using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86;
using static UnityEditor.Progress;

namespace Project
{
    public class Room_Generator : MonoBehaviour
    {
        [Header("Conditions")]
        public int numberOfRooms;
        int FinalnumberOfRooms;
        
        public int Padding;
        public int MaxAttempts;
        Grid grid;
        CorridorGeneration CG;
        CorridorWallHandle CWH;
        Testing test;
        Vector3 centertingVector;
        public List<GameObject> roomsObj;
        public List<GameObject> ObligatoryRoomsObj;
        public GameObject corridorPrefab;
        public List<GameObject> AddedRoomsObj;
        public List<Room> AddedRooms;
        public List<GameObject> coridorGO;
        public List<Vector2> occupied;
        public List<Vector2> boundcells;
        public List<Vector2> AllDoors;
        public List<Vector4> corridors;
        public List<int> connected;
        public List<family> families;
        public int numberofObligatoryRooms;
        
        int x;
        int y;
        GameObject chosenRoom;
        Vector2[] RoomCells;
        Vector2[] Doors;
        // Start is called before the first frame update
        public void RoomGen()
        {
            CWH = GetComponent<CorridorWallHandle>();
            families = new List<family>();
            //Line of Occupied states surrounding grid to avoid edge case
            test = GetComponent<Testing>();
            CG = GetComponent<CorridorGeneration>();
            test.GenerateGrid();
            grid = test.grid;
            //add Boundaries to occupied
            foreach (var cell in grid.bounds)
            {
                occupied.Add(cell);
            }
            numberofObligatoryRooms = ObligatoryRoomsObj.Count;
            //Room placement
            for (int i = 0; i < numberOfRooms; i++) 
            {
                centertingVector = new Vector3(0,0,0) / 2;
                Vector2 RoomSize = new Vector2();
                int attempts = 0;
                //Loop for attempting to place room if not possible after 10 attempts break generation loop
                
                while (attempts <= MaxAttempts)
                {
                    //RNG section
                    x = Mathf.RoundToInt(Random.Range(1, grid.width));
                    y = Mathf.RoundToInt(Random.Range(1, grid.height));
                    if (i > (numberofObligatoryRooms-1))
                    {
                        chosenRoom = roomsObj[Mathf.RoundToInt(Random.Range(0, roomsObj.Count))];
                    } 
                    else
                    {
                        chosenRoom = ObligatoryRoomsObj[i];  
                    }

                        
                    //Get roomSize
                    RoomSize = chosenRoom.GetComponent<Room>().size;
                    //CheckPossibleOccupiedCells
                    RoomCells = getRoomCells(RoomSize, x, y);

                    //LoopOverEach to check if X and Y matches any of the already occupied states
                    
                    if (DoesOverlap(RoomCells, occupied))
                    {
                        if (attempts == MaxAttempts)
                        {
                            break;
                        }
                        //if Overlap then 
                        //Debug.Log($"Attempt:{attempts}: OverLap!");
                        attempts++;
                        continue;
                    }
                    break;
                    
                }
                if(attempts==MaxAttempts)
                {
                    Debug.Log("MaxAttempts reached: Could not place room");
                    break;
                }

                if (RoomSize[0] % 2 != 0){centertingVector[0]=test.CellSize/2;}
                if (RoomSize[1] % 2 != 0){centertingVector[2]=test.CellSize/2;}
                // Get spawn pos
                Vector3 pos = grid.GetWorldPos(x, y)+ centertingVector ;
                //Instantiate room prefab
                GameObject room = Instantiate(chosenRoom, pos, Quaternion.identity);
                //Get Room Component of instantiated room
                Room roomComp = room.GetComponent<Room>();
                //Get Door positions

                //Add shit to arrays
                AddedRoomsObj.Add(room);
                AddedRooms.Add(roomComp);
                
                

                //Set new Room positions as occupied
                
                foreach (var item in RoomCells)
                {
                    int xPos = (int)item.x;
                    int yPos = (int)item.y;
                    grid.SetVal(xPos, yPos, 1);
                    occupied.Add(new Vector2(xPos, yPos));
                }

               
                // Get door positions and corridor positions
                List<Vector2> corridorPos = roomComp.CorridorStartPos;

                Doors = getDoorPositionswithCorridorStartPositions(roomComp.size, x, y, roomComp.DoorPos, corridors, i, corridorPos);
                
                //Store door in array
                foreach (var item in Doors)
                {
                    AllDoors.Add(item);
                }
                FinalnumberOfRooms = i;
                
            }

            numberOfRooms = FinalnumberOfRooms + 1;
            
            //SetDoorPosition Values
            foreach (var item in AllDoors)
            {
                grid.SetVal((int)item[0], (int)item[1], 10);
            }


            //REMINDER THAT WALLS DEPEND ON ORDER OF CORRIDOR GENERATION CALLED IN THIS FUNCTION 
            //put in corridor starts
            foreach (var item in corridors)
            {
                GameObject cor = Instantiate(corridorPrefab, grid.GetWorldPos((int)item[0], (int)item[1])+centertingVector, Quaternion.identity);
                coridorGO.Add(cor);
            }


            //Start connecting corridors
            for (int i = 0; i < numberOfRooms; i++)
            {

                List<int> availableRooms = new List<int>();
                //Create standard available rooms list
                for (int j = 0; j < numberOfRooms; j++) { 
                    availableRooms.Add(j);
                }
                family roomFam = null;
                //Iterate through all families to check if current room is already there
                
                //If current room is not already in a family create one
                if(!CheckIfInFamily(i, families,out roomFam)) {
                    roomFam = new family(new List<int> { i });
                    //Ground families
                    families.Add(roomFam);

                }


                //Subtract family rooms from available rooms
                availableRooms = availableRooms.Except(roomFam.rooms).ToList();


                int connectedRoom = 0;
                Vector4 endCorridor = Vector4.zero;
                Vector4 StartCorridor = Vector4.zero;
                //Find closest corridor available
                if (availableRooms.Count > 0 )
                {
                    //FOR TESTING
                    //connectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];


                    //Choose start corridor and end corridor
                    endCorridor = CG.FindClosestCorridor(i, corridors, availableRooms, out connectedRoom, out StartCorridor);

                    //Draw the Lines to visualize connections
                    Debug.DrawLine(grid.GetWorldPos((int)StartCorridor.x, (int)StartCorridor.y) + centertingVector, grid.GetWorldPos((int)endCorridor.x, (int)endCorridor.y)+ centertingVector, UnityEngine.Color.green, 100f);
                    CorridorPathfinding(StartCorridor, endCorridor, i);
                    

                }
                else
                {
                    break;
                }
                



                //Store family in variable
                // call merge func in family of ith Room 

                //Else:
                //Add connected room to family

                family CRfam = null;
                // if connected room is in family 
                if(CheckIfInFamily(connectedRoom, families,out CRfam))
                {
                    roomFam.mergeFams(CRfam,families);
                }
                else
                {
                    roomFam.rooms.Add(connectedRoom);
                }
                


            }

            CWH.TILECHECKK(corridors, coridorGO, grid);

            
        }


        public bool CheckIfInFamily(int i, List<family> fams, out family roomFam)
        {
            foreach (family fam in fams)
            {
                foreach (int room in fam.rooms)
                {
                    if (room == i)
                    {
                        roomFam = fam;
                        return true;

                    }
                }
            }
            roomFam = null;
            return false;
        }


        public void CorridorPathfinding(Vector4 start, Vector4 stop, int roomNumber)
        {
            List<Vector2> path = FindPath(start, stop);

            //Set Path
            foreach (var item in path)
            {
                grid.SetVal((int)item.x, (int)item.y, 3);
                GameObject cor = Instantiate(corridorPrefab, grid.GetWorldPos((int)item[0], (int)item[1]) + centertingVector, Quaternion.identity);
                coridorGO.Add(cor);
                corridors.Add(new Vector4(item.x, item.y, roomNumber, 0));
            }
   
            

        }

        public float CorridorAttempts;
        public List<Vector2> FindPath(Vector2 start, Vector2 stop)
        {
            List<Vector2> path = new List<Vector2>();
            float distance = Vector2.Distance(stop, start);
            Vector2 currentPos = start;
            List<Vector2> obstacles = new List<Vector2>();
            bool Obstacle = false;
            float lastDistance = 0;
            int attempts1 = 0;
            int attempts2 = 0;
            while(distance != 0 && attempts1 < CorridorAttempts)
            {
                Vector2 dirVec = (stop - currentPos).normalized;
                Vector2 checkDir = Vector2.zero;
                //Check which direction is larger x or y
                if (Mathf.Abs(dirVec.x) > Mathf.Abs(dirVec.y))
                {
                    //Check in the direction
                    checkDir = new Vector2(dirVec.x, 0).normalized;
                }
                //Otherwise
                else
                {
                    checkDir = new Vector2(0,dirVec.y).normalized;
                }
                while(attempts2 < CorridorAttempts)
                {
                    
                    Vector2 nextTile = checkDir + currentPos;
                    
                    int xPos = (int)nextTile.x;
                    int yPos = (int)nextTile.y;
                    
                   
                    //if in line with the room but checking the wrong direction
                    if ((dirVec.x == 0 || dirVec.y == 0) && Vector2.Dot(checkDir, dirVec) != 1)
                    {
                        break;
                    }

                    //If direction vector is perpendicular to a room blocking the path
                    else if((dirVec.x == 0 || dirVec.y == 0) && Vector2.Dot(checkDir, dirVec) == 1 && grid.GetVal(xPos,yPos) > 3)
                    {
                        Vector2 initialDir = checkDir;
                        List<int> pm = new List<int>{ -1, 1 };
                        checkDir = new Vector2( dirVec.y * pm[Random.Range(0,pm.Count)], dirVec.x * pm[Random.Range(0, pm.Count)]).normalized;
                        
                        Obstacle = true;
                        obstacles.Add(nextTile);
                        Vector2 obstacleDir = currentPos + initialDir;
                        int attempts3 = 0;
                        while (grid.GetVal((int)obstacleDir.x, (int)obstacleDir.y) > 3&& attempts3 < 10)
                        {
                            nextTile = checkDir + currentPos;
                            xPos = (int)nextTile.x;
                            yPos = (int)nextTile.y;

                            if (grid.GetVal(xPos, yPos) <= 3)
                            {
                                path.Add(nextTile);
                                currentPos = nextTile;
                            }
                            else
                            {
                                
                                Obstacle = true;
                                obstacles.Add(nextTile);
                                break;
                            }
                            attempts3 ++;
                        }
                    }


                    //If not an obstacle
                    else if (grid.GetVal(xPos, yPos) <= 3)
                    {
                        //Then continue in this direction
                        path.Add(nextTile);
                        currentPos = nextTile;

                        //grid.SetVal(xPos, yPos, 3);
                        //Instantiate(corridorPrefab, grid.GetWorldPos(xPos, yPos) + centertingVector, Quaternion.identity);


                    }

                    //Force 
                    else if (Obstacle)
                    {
                        print("HIT OBSTACLE");
                        break;
                    }

                    else
                    {
                        checkDir = new Vector2(Mathf.Abs(checkDir.y)*dirVec.x, dirVec.y*Mathf.Abs(checkDir.x)).normalized;
                        lastDistance = Vector2.Distance(currentPos, stop);
                        Obstacle = true;
                        obstacles.Add(nextTile);
                        
                    }
                    dirVec = stop - currentPos;
                    attempts2++;
                }
                attempts1++;
            }
            if(attempts1 == CorridorAttempts)
            {
                //Debug.LogError("Couldnt find path");
            }

            return path;
        }

        


        public Vector2[] getDoorPositionswithCorridorStartPositions(Vector2 size, int x, int y,List<int> doorArray,List<Vector4>CorridorArray, int roomIndex, List<Vector2> corridorPos)
        {
            Vector2[] arrayofCoords = new Vector2[doorArray.Count];
            float xRange = (((int)size[0] - 1) / 2f);
            float yRange = (((int)size[1] - 1) / 2f);
            // range compensators for even length rooms
            float ex = 0;
            float ey = 0;
            if (size[0] % 2 == 0){ex=+0.5f;}
            if (size[1] % 2 == 0){ey=+0.5f;}
            int f = 0;
            int d = 0;
            //Set occupied cells to occupied
            for (int j = (int)(xRange-ex); j >= (int)(-xRange-ex); j--)
            {
                for (int k = (int)(yRange-ey); k >= (int)(-yRange-ey); k--)
                {
                    
                    foreach (int door in doorArray)
                    {
                        grid.SetVal(x + j, y + k, 5);
                        if (f == door)
                        {
                            Vector2 currentstartcorridor= (1/test.CellSize)*corridorPos[d];
                            Vector2 doorPos = new Vector2(x + j, y + k);
                            arrayofCoords[d] = doorPos;
                            CorridorArray.Add(new Vector4(x+currentstartcorridor.x,y+currentstartcorridor.y, roomIndex, 0));
                            grid.SetVal(x+(int)currentstartcorridor.x, y+(int)currentstartcorridor.y, 3);
                            d++;
                        }
                    }

                    f++;

                }
            }
            return arrayofCoords;
        }

        public Vector2 getCorridorStartPositions(Vector2 doorPos, Vector2 middle)
        {
            Vector2 dirVec = (doorPos - middle).normalized;
            return dirVec+doorPos;
        }


        public Vector2[] getRoomCells(Vector2 size, float x , float y)
        {

            Vector2[] arrayofCoords = new Vector2[(int)((size[0]+2*Padding) * (size[1] + 2 * Padding))];
            float ex = 0; //evenlength corrections
            float ey = 0;
            if (size[0] % 2 == 0){ex=+0.5f;}
            if (size[1] % 2 == 0){ey=+0.5f;}
            float xRange = (((int)size[0] - 1) / 2);
            float xRangePadded = xRange+Padding+ex;
            float yRange = (((int)size[1] - 1) / 2);
            float yRangePadded = yRange + Padding+ey;

            
            int f = 0;
            //Set occupied cells to occupied
            for (float j = xRangePadded; j >= -xRangePadded; j--)
            {
                for (float k = yRangePadded; k >= -yRangePadded; k--)
                {
                    arrayofCoords[f] = new Vector2((int)(x+j),(int)(y+k));
                    //print(arrayofCoords[f]);
                    
                    f++;
                    
                }
            }
            return arrayofCoords;
        }

        public bool DoesOverlap(Vector2[] roomCells, List<Vector2> occupiedList)
        {
            foreach (Vector2 cell in roomCells)
            {
                foreach (var item in occupiedList)
                {
                    if (item == cell)
                    {

                        return true;

                    }
                }

            }
            return false;
        }

        
        
    }
}
