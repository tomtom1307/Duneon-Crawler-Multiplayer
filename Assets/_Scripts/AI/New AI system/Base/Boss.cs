using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Boss : Enemy
    {
        //
        public int currentPhase;

        public override void Start()
        {
            base.Start();
            currentPhase = 1;

        }


        // Health percentage 
        public float[] PhaseTriggerHealthValues;

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

        public virtual bool CheckPhaseCondition(int phase)
        {
            if(CurrentHealth.Value <= PhaseTriggerHealthValues[phase - 1])
            {
                return true;
            }
            return false;
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
