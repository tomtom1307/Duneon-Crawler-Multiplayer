using Project.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI instance;

    public GameObject Inventory;
    public PlayerAttack PA;
    public KeyCode InventoryKey;
    public PlayerCam Pc;

    bool InUI;
    bool Active;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        PA = FindAnyObjectByType<PlayerAttack>();
        Active = false;
        Inventory.SetActive(Active);
        Pc = FindAnyObjectByType<PlayerCam>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(InventoryKey)&&!InUI) { 
            Active = !Active;
            PlayerInUI(Active);
            Inventory.SetActive(Active);
            
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            Inventory.SetActive(false);
            PlayerInUI(false);
        }

        



    }

    public void PlayerInUI(bool x)
    {

        if (Inventory.GetComponent<Inventory>().Active)
        {
            PA.enabled = !x;
        }
        Pc.enabled= !x;
        Cursor.visible = x;
        if (x)
        {
            InUI = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            InUI = false;
        }

        Cursor.visible = x;
    }





}
