using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class ShieldGeneratorTentacle : DamageableThing
    {
        Animator anim;
        public override void Start()
        {
            anim = GetComponent<Animator>();
            base.Start();
            Trigger();
            
        }

        public void Trigger()
        {
            Actions.ShieldGeneratorTentacleUpdate(this, true);
        }


        public void Update()
        {
            
        }

        [ServerRpc(RequireOwnership = false)]
        public override void TakeDamageServerRpc(float Damage)
        {
            base.TakeDamageServerRpc(Damage);
            OnDamaged();
        }

        public override void OnDamaged()
        {
            base.OnDamaged();
            anim.SetTrigger("Hit");
            if(CurrentHealth.Value <= 0)
            {

                anim.SetBool("hide", true);
                Actions.ShieldGeneratorTentacleUpdate(this, false);
            }
            
        }

    }
}