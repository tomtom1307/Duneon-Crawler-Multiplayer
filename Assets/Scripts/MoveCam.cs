using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public Transform playerPos;

    


    void Update()
    {
        if (playerPos != null)
        {
            transform.position = playerPos.position;
        }
        
    }
}
