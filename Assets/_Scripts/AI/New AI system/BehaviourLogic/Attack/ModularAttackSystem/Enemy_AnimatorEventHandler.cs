using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_AnimatorEventHandler : MonoBehaviour
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
