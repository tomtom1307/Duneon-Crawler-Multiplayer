using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class BruteGoblin : NavMeshEnemy
    {
        [ServerRpc(RequireOwnership = false)]
        public override void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            base.DoDamageServerRpc(Damage, headshot);

            animator.Play("HitFlinch", -1, 0f);
        }


    }
}
