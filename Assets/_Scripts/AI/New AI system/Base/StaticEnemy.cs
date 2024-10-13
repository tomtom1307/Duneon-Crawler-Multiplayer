using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class StaticEnemy : Enemy
    {
        MeshRenderer MR;
        // Start is called before the first frame update
        public override void Start()
        {
            MR = gameObject.GetComponentInChildren<MeshRenderer>();
            origColors = MR.materials;
            whites = MR.materials;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            StateMachine.Initialize(AttackState);
        }


        public override void FlashStart()
        {
            MR.SetMaterials(whites.ToList());
        }

        public override void FlashEnd()
        {
            MR.SetMaterials(origColors.ToList());
        }

        [ServerRpc(RequireOwnership = false)]
        public override void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            base.DoDamageServerRpc(Damage, headshot);
            if (CurrentHealth.Value <= 0)
            {
                Die();
            }
        }



    }
}
