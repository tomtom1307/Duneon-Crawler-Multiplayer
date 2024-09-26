using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class TestBoss : Boss
    {
        

        public override void OnDamage()
        {
            base.OnDamage();
            switch (currentPhase)
            {
                case(1):
                    CheckPhaseCondition(1);
                    transform.DOScale(1.5f * transform.localScale, 1f);
                    
                    break;
                case(2):
                    CheckPhaseCondition(2);
                    transform.DOScale(1.5f * transform.localScale, 1f);
                    break;
                case(3):
                    CheckPhaseCondition(3);
                    transform.DOScale(1.5f * transform.localScale, 1f);
                    break;
            }



        }

        public override void AttackExit()
        {
            base.AttackExit();
        }

        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
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
