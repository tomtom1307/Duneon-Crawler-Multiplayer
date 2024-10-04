using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DungeonDoorLogic : MonoBehaviour
    {

        Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        public void TriggerDoor(bool X)
        {
            animator.SetBool("Open", X);
        }

    }
}
