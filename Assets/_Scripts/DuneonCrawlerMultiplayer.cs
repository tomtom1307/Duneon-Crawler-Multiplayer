using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class DuneonCrawlerMultiplayer : NetworkBehaviour
    {
        public static DuneonCrawlerMultiplayer instance { get; private set; }

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallBack;
            NetworkManager.Singleton.StartHost();
        }

        private void NetworkManager_ConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest connectApprovalrequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
        {
            print("Connection Approval");
            //Check if game is in a state where 
            connectionApprovalResponse.Approved = true;
        }



        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            print("Starting Client");
        }



    }
}
