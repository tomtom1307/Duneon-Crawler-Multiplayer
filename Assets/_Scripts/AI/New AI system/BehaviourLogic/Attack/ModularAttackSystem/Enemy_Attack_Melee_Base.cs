using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Enemy_Attack_Melee_Base : Enemy_Attack
    {
        

        //Detection Logic 
        public void Detect_Player_Collider(Enemy_Attack_ColliderDetector detector)
        {
            if (detector != null)
            {
                detector.TriggerCollider(true);
            }
        }

        public void DeactivateAttackCollider(Enemy_Attack_ColliderDetector detector)
        {
            detector.TriggerCollider(false);
        }


        

    }
}
