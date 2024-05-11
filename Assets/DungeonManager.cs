using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class DungeonManager : NetworkBehaviour
    {
        [SerializeField] private Transform PlayerPrefab;




        private void Awake()
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadeventCompleted;
        }



        private void SceneManager_OnLoadeventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {

            DungeonGenStarterClientRpc();
            print("Worked");
        }




        [ClientRpc]
        public void DungeonGenStarterClientRpc()
        {
            if (IsHost)
            {
                print("HostGen");
                Seeding.instance.HostGen();
            }
            else
            {
                print("ClientGen");
                Seeding.instance.ReadSeed();
            }
        }


    }
}
