using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Stairs : MonoBehaviour
    {
        private CapsuleCollider playerCollider;
        private Rigidbody playerBody;
        private float colRadius;
        private Vector3 lowerCastPos;
        private Vector3 upperCastPos;
        private Vector3 lowerCastDisp;
        private Vector3 upperCastDisp;
        private Vector3 castDirection;
        private LayerMask layerMask;
        private float horizontalInput;
        private float verticalInput;
        private Transform orientation;
        [SerializeField] float maxHeight;
        [SerializeField] float checkDistance;
        [SerializeField] float stepHeight;


        void Start()
        {
            playerCollider = GetComponent<CapsuleCollider>();

            // Getting collider radius
            colRadius = playerCollider.radius;

            // Defining lower Raycast displacement from collider centre
            lowerCastDisp = new Vector3(0, -playerCollider.height/2, 0);

            // Defining upper Raycast displacement from lowercast
            upperCastDisp = new Vector3(0, maxHeight, 0);

            // Getting reference to Rigidbody
            playerBody = GetComponent<Rigidbody>();

            // Getting reference to orientation Transform
            orientation = GetComponent<PlayerMovement>().orientation;

            // Initializing LayerMask
            layerMask = LayerMask.GetMask("Ground");
        }

        void FixedUpdate()
        {
            // Get movement input and define raycast direction
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            castDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            // Set upper raycast to specified height
            upperCastDisp.y = maxHeight;

            // Set raycast origins
            lowerCastPos = transform.position + playerCollider.center + lowerCastDisp;
            upperCastPos = lowerCastPos + upperCastDisp;

            // Draw raycasts
            Debug.DrawRay(lowerCastPos, castDirection * (colRadius + checkDistance), Color.red, Time.deltaTime);
            Debug.DrawRay(upperCastPos, castDirection * (colRadius + checkDistance), Color.red, Time.deltaTime);

            // if lower raycast hits but upper does not: Move player up.
            RaycastHit lowerhit;
            if (Physics.Raycast(lowerCastPos, castDirection, out lowerhit, (colRadius + checkDistance), layerMask))
            {
                Debug.Log("lower cast hit");
                RaycastHit upperhit;
                if (!Physics.Raycast(upperCastPos, castDirection, out lowerhit, (colRadius + checkDistance), layerMask))
                {
                    Debug.Log("player moved");
                    playerBody.MovePosition(new Vector3(playerBody.position.x, playerBody.position.y + stepHeight, playerBody.position.z));
                }
            }
        }
    }
}
