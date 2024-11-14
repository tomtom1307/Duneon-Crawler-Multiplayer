using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class Enemy_AnimatorEventHandler : NetworkBehaviour
    {
        Enemy enemy;

        private void Start()
        {
            enemy = GetComponent<Enemy>();
        }

        public void AnimationTrigger(Enemy.AnimationTriggerType triggerType)
        {
            enemy.AnimationTriggerEvent(triggerType);
        }

        public void AttackLogic()
        {
            enemy.DoAttackLogic();
        }

        public void StartHitDetection()
        {
            enemy.AttackDetect();
        }

        public void StopHitDetection()
        {
            enemy.StopDetect();
        }



    }
}
