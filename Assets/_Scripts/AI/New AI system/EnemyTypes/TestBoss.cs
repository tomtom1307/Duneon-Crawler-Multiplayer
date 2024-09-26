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
                    if (CheckPhaseCondition(1))
                    {
                        currentPhase++;


                        AttackDamage *= 1.5f;
                        transform.DOScale(1.5f * transform.localScale, 1f);
                    }
                    
                    
                    break;
                case(2):
                    if (CheckPhaseCondition(2))
                    {
                        currentPhase++;

                        AttackDamage *= 1.5f;
                        transform.DOScale(1.5f * transform.localScale, 1f);
                    }
                    break;
                case (3):
                    if (CheckPhaseCondition(3))
                    {
                        currentPhase++;

                        DamageReduction = 0.1f;
                        AttackDamage *= 1.5f;
                        transform.DOScale(1.5f * transform.localScale, 1f);
                    }
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
