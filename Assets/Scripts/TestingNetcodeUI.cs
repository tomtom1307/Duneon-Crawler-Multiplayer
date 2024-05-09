using System.Collections;
using System.Collections.Generic;
using Project;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;
    [SerializeField] public PlayerCam PC;


    private void Awake()
    {
        PC.enabled = false;
        startHostButton.onClick.AddListener(() =>
        {   
            Hide();
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            PC.enabled = enabled;
            try{Seeding.instance.HostGen();}
            catch(Exception e){Debug.Log("Could not call RoomGen: " + e.ToString());}
            
        });
        startClientButton.onClick.AddListener(() =>
        {
            Hide();
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            Seeding.instance.ReadSeed();
            PC.enabled = enabled;
        });
    }

    

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
