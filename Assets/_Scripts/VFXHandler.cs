using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class VFXHandler : MonoBehaviour
    {
        public GameObject Attack1PS;
        public Transform EffectOrigin;
        public float Speed;
        public GameObject Attack2PS;
        public LayerMask onTouch;
        Transform target; 
        public float Distance;
        GameObject Visual;
        public void Attack1(Vector3 EndPos)
        {
            Visual = Instantiate(Attack1PS, EffectOrigin.position, Quaternion.identity);
            MoveToTarget MT = Visual.GetComponent<MoveToTarget>();
            MT.speed = Speed;
            MT.target = EndPos;
            

            
        }

        public void Attack2(float Scale)
        {
            Visual = Instantiate(Attack2PS, EffectOrigin.position-Vector3.up, Quaternion.Euler(-90,0,0));
            Visual.transform.localScale = Scale * Vector3.one;
            Destroy(Visual,4);

        }


        private void Update()
        {


        }
    }
}
