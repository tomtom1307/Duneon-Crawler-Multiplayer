using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Project
{
    public class Enemy_Attack : ScriptableObject
    {
        public string AttackAnimationName;
        public int AnimationIntValue;
        public float Damage;
        public GameObject VFX;

        // Base Attack that Does nothing so far lol
        public virtual void Attack(PlayerStats ps, Enemy enemy)
        {

        }

        public virtual void playerHitLogic(PlayerStats ps, Enemy enemy)
        {
            
        }

        //Basic 
        public void BaseDetect(){}


        public virtual void Detect(Enemy enemy){}

        public virtual void StopDetecting(Enemy enemy) {}



        // Melee 
        public virtual void Detect(Enemy_Attack_ColliderDetector detector){}

        public virtual void StopDetecting(Enemy_Attack_ColliderDetector detector){}
    }
}
