using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class AttachToCam : NetworkBehaviour
    {
        Transform camPos;
        public Vector3 offset;

        private void Start()
        {
            
            camPos = Camera.main.transform.Find("HeadFollow").transform;


        }

        private void Update()
        {
            transform.position = camPos.position + offset;
        }
    }
}