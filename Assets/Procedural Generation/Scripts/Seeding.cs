using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

namespace Project
{
    
    
    public class Seeding : NetworkBehaviour 
    
    {
        public static Seeding instance;

        public Room_Generator Room_Generator;
        private int CurrentSeed;
        public NetworkVariable<int> GeneratedSeed = new NetworkVariable<int>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        private void Awake() 
        {
            // If there is an instance, and it's not me, delete myself.
            if (instance != null && instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                instance = this; 
            } 
            Room_Generator = GetComponent<Room_Generator>();
        }
        public void GenSeed() 
            {
                CurrentSeed = Random.Range(0, 100000000);
                Random.InitState(CurrentSeed);
                GeneratedSeed.Value = CurrentSeed;
                Room_Generator.RoomGen();
            }
        public void ReadSeed()
        {
            Random.InitState(GeneratedSeed.Value);
            Room_Generator.RoomGen();
        }
    }
}
