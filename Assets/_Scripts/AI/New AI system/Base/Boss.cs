using DG.Tweening;
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

        public virtual void Phase1()
        {
            
        }


        public virtual void Phase2()
        {
            
        }

        public virtual void Phase3()
        {
            
        }
        public virtual void Phase4()
        {
            
        }
        public virtual void Phase5()
        {
            
        }


        public override void OnDamage()
        {
            base.OnDamage();
            switch (currentPhase)
            {
                case (1):
                    if (CheckPhaseCondition(1))
                    {
                        Phase2();
                    }
                    break;
                case (2):
                    if (CheckPhaseCondition(2))
                    {
                        Phase3();
                    }
                    break;
                case (3):
                    if (CheckPhaseCondition(3))
                    {
                        Phase4();
                    }
                    break;
            }
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
