using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class MoveToTarget : MonoBehaviour
    {
        public Vector3 target;
        public float speed;
        public GameObject onDestroySpawn;
        public void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
            
            if (Vector3.Distance(transform.position, target) < 1) 
            {
                
                if(onDestroySpawn!= null)
                {
                    var bum = Instantiate(onDestroySpawn, transform.position, Quaternion.identity);
                    Destroy(bum,0.3f);
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
                
                
            
            }
        }
    }
}
