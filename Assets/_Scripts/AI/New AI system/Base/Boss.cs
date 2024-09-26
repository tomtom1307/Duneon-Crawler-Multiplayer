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


        // Health percentage to trigger next phase 
        public float[] PhaseTriggerHealthPercentage;

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
            if(CurrentHealth.Value <= 0.01f * PhaseTriggerHealthPercentage[phase - 1] * MaxHealth)
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
