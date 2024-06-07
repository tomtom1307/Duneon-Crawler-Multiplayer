using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class LeverLogic : _Interactable
    {
        Animator anim;
        [HideInInspector] public EnemySpawnTrap EST;
        [HideInInspector] public int Channel;

        private void Start()
        {
            anim = GetComponent<Animator>();
        }


        protected override void Interact()
        {
            base.Interact();
            anim.SetBool("Flip", true);
            EST.OnActivate(Channel);

            Destroy(this);
        }
    }
}
