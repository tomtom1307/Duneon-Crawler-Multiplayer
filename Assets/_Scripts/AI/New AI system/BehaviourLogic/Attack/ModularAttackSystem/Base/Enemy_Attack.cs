using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack : ScriptableObject
    {
        public string AttackAnimationName;
        public int AnimationIntValue;
        public float Damage;

        //Basic 
        public void BaseDetect(){}


        public virtual void Detect(Enemy enemy){}

        public virtual void StopDetecting(Enemy enemy) {}



        // Melee 
        public virtual void Detect(Enemy_Attack_ColliderDetector detector){}

        public virtual void StopDetecting(Enemy_Attack_ColliderDetector detector){}
    }
}
