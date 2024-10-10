using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class AirLockSystem : NetworkBehaviour
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
            Actions.InitializeMultiplayerStuffinScene += InitializeDoor;
            Players = new List<PlayerMovement>();
            print("Triggered Thingy");
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
                if(Players.Count == GameManager.instance.NumberOfPlayers.Value)
                {
                    PrimeAirLock();
                }

            }
        }

        public void PrimeAirLock()
        {
            EntranceDoor.TriggerDoorServerRpc(false);
            UnlockLeverServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void UnlockLeverServerRpc()
        {
            UnlockLeverClientRpc();
        }

        [ClientRpc]
        public void UnlockLeverClientRpc()
        {
            EntranceLever.Lock = false;
        }

        public void InitializeDoor()
        {
            EntranceDoor.TriggerDoorServerRpc(true);
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
