using Project.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public GameObject Inventory;
    public PlayerAttack PA;
    public KeyCode InventoryKey;
    public PlayerCam Pc;

    bool Active;
    // Start is called before the first frame update
    void Start()
    {
        PA = FindAnyObjectByType<PlayerAttack>();
        Active = false;
        Inventory.SetActive(Active);
        Pc = FindAnyObjectByType<PlayerCam>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(InventoryKey) || Input.GetKeyDown(KeyCode.Escape)) { 
            Active = !Active;
            if (Inventory.GetComponent<Inventory>().Active)
            {
                PA.enabled = !Active;
            }
            
            Inventory.SetActive(Active);
            Pc.enabled = !Active;
            if(Active)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = Active;
        }

        
    }
}
