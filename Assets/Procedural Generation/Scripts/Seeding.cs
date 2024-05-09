using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;

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
        public void HostGen() 
        {
            CurrentSeed = Random.Range(0, 100000000);
            Random.InitState(CurrentSeed);
            GeneratedSeed.Value = CurrentSeed;
            Room_Generator.RoomGen();
        }

        public void ReadSeed()
        {
            StartCoroutine(Loading(ClientGen));
        }
        
        public void ClientGen()
        {
            Random.InitState(GeneratedSeed.Value);
            Room_Generator.RoomGen();
        }

        public void NoRoomGen()
        {
            GeneratedSeed.Value = 0;
        }
        public IEnumerator Loading(Action methodName)
        {
            float f = 0f; //Kill coroutine after 10s wait
            while(GeneratedSeed.Value == 0 && f < 10f)
            {
                yield return new WaitForSeconds(0.5f);
                f = f + 0.5f;
            }
                if (f < 10f)
                {
                    Debug.Log("Seed updated and Client called RoomGen. Seed: "+ GeneratedSeed.Value);
                    methodName();
                }
                else{Debug.Log("Seed did not update. RoomGen cancelled.");}

                yield return null;
        }

    }
}
