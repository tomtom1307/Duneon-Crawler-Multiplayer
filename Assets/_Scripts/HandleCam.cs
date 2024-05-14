using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class HandleCam : NetworkBehaviour
{
    public Transform orientation;
    public Transform Headposition;
    public MoveCam MC;
    public PlayerCam PC;
    public GameObject PM;
    public GameObject Hand;
    public GameObject PlayerWeapon;
    public int PlayerWeaponLayerMask;

    private void Start()
    {
        if(IsLocalPlayer)
        {
            Hand = FindObjectOfType<HandStuff>().gameObject;
            MC = FindObjectOfType<MoveCam>();
            PC = FindObjectOfType<PlayerCam>();
            MC.playerPos = Headposition;
            PC.orientation = orientation;
            if(PlayerWeapon != null )
            {
                //GameObject PW = Instantiate(PlayerWeapon, Hand.transform);
                //foreach (Transform child in PW.transform)
                //{
                //    child.gameObject.layer = PlayerWeaponLayerMask;
                //}
                PlayerWeapon.SetActive(false);
            }



            MC.enabled = true;
            PM.SetActive(false);
            
            
        }
    }


    
}
