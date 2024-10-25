using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Croblin : NavMeshEnemy
    {
        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            EnemyChaseInstance.Initialize(gameObject, this);


            StateMachine.Initialize(AttackState);
        }
    }
}
