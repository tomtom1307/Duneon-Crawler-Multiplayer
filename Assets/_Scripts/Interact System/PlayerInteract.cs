using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class PlayerInteract : NetworkBehaviour
    {
        private Camera cam;
        [SerializeField] LayerMask WhatIsinteractble;
        [SerializeField]private float distance = 3f;
        public TextMeshProUGUI text;
        public GameObject HUD;
        
        // Start is called before the first frame update
        void Start()
        {
            if (!IsOwner)
            {
                Destroy(HUD);
                return;
            }
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                
                return;
            }
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,distance,WhatIsinteractble))
            {
                text.enabled = true;
                _Interactable interactable;
                if (hit.collider.gameObject.TryGetComponent<_Interactable>(out interactable))
                {
                    text.text = interactable.Prompt;

                    if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.F))
                    {
                        interactable.BaseInteract(this.gameObject);
                        text.text = "";
                    }
                }
            }
            else
            {
                text.enabled = false;
            }
        }
    }
}
