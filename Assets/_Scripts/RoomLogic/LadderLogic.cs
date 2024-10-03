using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

namespace Project
{
    public class LadderLogic : NetworkBehaviour
    {

        public float climbSpeed;
        public List<PlayerMovement> PM;
        
        private void OnTriggerEnter(Collider other)
        {

            PlayerMovement PMinst;
            if (other.gameObject.TryGetComponent<PlayerMovement>(out PMinst))
            {
                PM.Add(PMinst);
            }

            

        }

        private void Update()
        {

            


            if (PM.Count > 0 && Input.GetKey(KeyCode.Space))
            {
                foreach (var item in PM)
                {
                    item.rb.velocity = Vector3.up * climbSpeed;
                }
            }
        }


        private void OnTriggerExit(Collider other)
        {
            PlayerMovement PMinst;
            if (other.gameObject.TryGetComponent<PlayerMovement>(out PMinst))
            {
                PM.Remove(PMinst);
            }



        }




    }
}
