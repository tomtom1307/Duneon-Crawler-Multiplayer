using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class LeverLogic : _Interactable
    {
        Animator anim;
        EnemySpawnTrap EST;
        private void Start()
        {
            anim = GetComponent<Animator>();
            EST = GetComponentInParent<EnemySpawnTrap>();
        }


        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Flip", true);
            EST.DoSomething();

            Destroy(this);
        }
    }
}
