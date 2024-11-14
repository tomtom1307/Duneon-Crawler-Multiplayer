using System.Collections;
using System.Collections.Generic;
using Project;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestingNetcodeUI : NetworkBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] public PlayerCam PC;

    [SerializeField] private NetworkVariable<int> playerNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        PC.enabled = false;
        startHostButton.onClick.AddListener(() =>
        {
            
            
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            UpdatePlayerNumberServerRpc();
            Hide();
            PC.enabled = enabled;
            try{Seeding.instance.HostGen();}
            catch(Exception e)
            {
                Debug.Log("Could not call HostGen: " + e.ToString());
                Debug.Log("Setting seed to 0");
                Seeding.instance.NoRoomGen();
            }
            if (Actions.InitializeMultiplayerStuffinScene != null)
            {
                Actions.InitializeMultiplayerStuffinScene();
            }
        });
        startClientButton.onClick.AddListener(() =>
        {

            
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            //UpdatePlayerNumberServerRpc();
            Hide();
            PC.enabled = enabled;
            try{Seeding.instance.ReadSeed();}
            catch(Exception e){Debug.Log("Could not call ReadSeed: " + e.ToString());}
        });
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerNumberServerRpc()
    {
        if(!IsHost)
        {
            return;
        }
        playerNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
