using System.Collections;
using System.Collections.Generic;
using Project;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

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
            Debug.Log("HOST");
            NetworkManager.Singleton.StartHost();
            PC.enabled = enabled;
            Seeding.instance.GenSeed();
            Hide();

        });
        startClientButton.onClick.AddListener(() =>
        {
            Debug.Log("Client");
            NetworkManager.Singleton.StartClient();
            PC.enabled = enabled;
            Seeding.instance.ReadSeed();
            Hide();
        });
    }


    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
