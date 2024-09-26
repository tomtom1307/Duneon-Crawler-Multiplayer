using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Boss : Enemy
    {
        public int phase;
        public Vector2[] HealthValues;

        public override void AttackExit()
        {
            base.AttackExit();
        }

        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
        }

        public override void OnDamage()
        {
            base.OnDamage();
        }

        public override void OnDeathTellSpawner()
        {
            base.OnDeathTellSpawner();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
