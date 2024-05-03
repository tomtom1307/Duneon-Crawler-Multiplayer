using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.X86;

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
        Testing test;
        Vector3 centertingVector;
        public List<GameObject> roomsObj;
        public GameObject corridorPrefab;
        public List<GameObject> AddedRoomsObj;
        public List<Room> AddedRooms;
        public List<Vector2> occupied;
        public List<Vector2> AllDoors;
        public List<Vector3> corridors;
        public List<int> connected;
        public List<family> families;
        
        int x;
        int y;
        GameObject chosenRoom;
        Vector2[] RoomCells;
        Vector2[] Doors;
        // Start is called before the first frame update
        void Start()
        {
            families = new List<family>();
            //Line of Occupied states surrounding grid to avoid edge case
            test = GetComponent<Testing>();
            CG = GetComponent<CorridorGeneration>();
            test.GenerateGrid();
            grid = test.grid;
            centertingVector = new Vector3(test.CellSize, 0, test.CellSize) / 2;
            //add Boundaries to occupied
            foreach (var cell in grid.bounds)
            {
                occupied.Add(cell);
            }
            

            for (int i = 0; i < numberOfRooms; i++) 
            {
                int attempts = 0;
                //Loop for attempting to place room if not possible after 10 attempts break generation loop
                
                while (attempts <= MaxAttempts)
                {
                    //RNG section
                    x = Mathf.RoundToInt(Random.Range(1, grid.width));
                    y = Mathf.RoundToInt(Random.Range(1, grid.height));
                    chosenRoom = roomsObj[Mathf.RoundToInt(Random.Range(0, roomsObj.Count))];


                    //Get roomSize
                    Vector2 RoomSize = chosenRoom.GetComponent<Room>().size;
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
                        Debug.Log($"Attempt:{attempts}: OverLap!");
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
                Doors = getDoorPositionswithCorridorStartPositions(roomComp.size, x, y, roomComp.DoorPos, corridors, i);
                
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

            //put in corridor starts
            foreach (var item in corridors)
            {
                Instantiate(corridorPrefab, grid.GetWorldPos((int)item[0], (int)item[1])+centertingVector, Quaternion.identity);
            }

            int n = 0;
            //Start connecting corridors
            for (int i = 0; i < numberOfRooms; i++)
            {
                print(i);
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
                //Find closest corridor available
                if (availableRooms.Count > 0 )
                {
                    //FOR TESTING
                    //connectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];

                    connectedRoom = 

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
            //For some reason additional room in family
            foreach (family fam in families)
            {
                print("fam:" + n);
                foreach (var room in fam.rooms)
                {
                    print(room);
                }
            }
            
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


        public Vector2[] getDoorPositionswithCorridorStartPositions(Vector2 size, int x, int y,List<int> doorArray,List<Vector3>CorridorArray, int roomIndex)
        {
            Vector2[] arrayofCoords = new Vector2[doorArray.Count];
            int xRange = (((int)size[0] - 1) / 2);
            int yRange = (((int)size[1] - 1) / 2);

            int f = 0;
            int d = 0;
            //Set occupied cells to occupied
            for (int j = xRange; j >= -xRange; j--)
            {
                for (int k = yRange; k >= -yRange; k--)
                {
                    
                    foreach (int door in doorArray)
                    {
                        grid.SetVal(x + j, y + k, 2);
                        if (f == door)
                        {
                            
                            Vector2 doorPos = new Vector2(x + j, y + k);
                            arrayofCoords[d] = doorPos;
                            Vector2 corridorPos = getCorridorStartPositions(doorPos, new Vector2(x, y));
                            CorridorArray.Add(new Vector3(corridorPos.x,corridorPos.y, roomIndex));
                            grid.SetVal((int)corridorPos[0], (int)corridorPos[1], 3);
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


        public Vector2[] getRoomCells(Vector2 size, int x , int y)
        {

            Vector2[] arrayofCoords = new Vector2[(int)((size[0]+2*Padding) * (size[1] + 2 * Padding))];
            int xRange = (((int)size[0] - 1) / 2);
            int xRangePadded = xRange+Padding;
            int yRange = (((int)size[1] - 1) / 2);
            int yRangePadded = yRange + Padding;

            int f = 0;
            //Set occupied cells to occupied
            for (int j = xRangePadded; j >= -xRangePadded; j--)
            {
                for (int k = yRangePadded; k >= -yRangePadded; k--)
                {
                    arrayofCoords[f] = new Vector2(x+j,y+k);
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
