using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class TestBoss : Boss
    {

        public override void Phase2()
        {
            base.Phase2();
            currentPhase++;


            AttackDamage *= 1.5f;
            transform.DOScale(1.5f * transform.localScale, 1f);
        }

        public override void Phase3()
        {
            base.Phase3();
            currentPhase++;


            AttackDamage *= 1.5f;
            transform.DOScale(1.5f * transform.localScale, 1f);
        }

        public override void Phase4()
        {
            base.Phase4();
            currentPhase++;

            DamageReduction = 0.3f;
            AttackDamage *= 1.5f;
            transform.DOScale(1.5f * transform.localScale, 1f);
        }




        public override void OnDamage()
        {
            base.OnDamage();
            



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
