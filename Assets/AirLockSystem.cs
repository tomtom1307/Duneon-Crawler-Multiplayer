using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class AirLockSystem : MonoBehaviour
    {
        List<PlayerMovement> Players;


        //Entrance to airlock
        public DungeonDoorLogic EntranceDoor;

        //Exit To enter the room
        public DungeonDoorLogic ExitDoor;

        //Lever to enter the room 
        public DoorLeverLogic EntranceLever;

        private void Start()
        {
            Players = new List<PlayerMovement>();
            EntranceDoor.TriggerDoor(true);
            EntranceLever.Lock = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement playerMovement;
            //Add Players to a list when enter the collider and check if the condition is met
            if (other.gameObject.TryGetComponent<PlayerMovement>(out  playerMovement))
            {
                Players.Add(playerMovement);


                //Check How many Players are required to activate the room 
                if(Players.Count == 1)
                {
                    PrimeAirLock();
                }

            }
        }

        public void PrimeAirLock()
        {
            EntranceDoor.TriggerDoor(false);
            EntranceLever.Lock = false;
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                Players.Remove(playerMovement);
                
            }
        }
    }
}
