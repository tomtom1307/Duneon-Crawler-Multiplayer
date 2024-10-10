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
        public virtual void Detect(){}

        public virtual void StopDetecting() { }


        // For Melee Detection 
        public virtual void Detect(Enemy_Attack_ColliderDetector colliderDetector) { }
        

        public virtual void StopDetecting(Enemy_Attack_ColliderDetector colliderDetector) { }
        

        
    }
}
