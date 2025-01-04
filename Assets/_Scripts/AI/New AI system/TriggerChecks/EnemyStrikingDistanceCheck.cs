using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class EnemyStrikingDistanceCheck : NetworkBehaviour
    {
        private Enemy enemy;
        private float timer;


        private void Start()
        {
            enemy = GetComponent<Enemy>();
            

        }

        private void Update()
        {
            if (!IsOwner) return;
            float distance = Vector3.Distance(enemy.target.position, transform.position);
            

            
            
            if (distance < enemy.AttackDistance)
            {
                enemy.SetStrikingDistanceBool(true);

                Debug.Log(enemy.IsWithinStrikingDistance + "yes I am within striking distance");
                print(enemy.target);
                //print("InRange");
                timer = 0;
            }
            

            else if(distance > enemy.AttackDistance+1)
            {
                timer += Time.deltaTime;
                if(timer > 1 - enemy.aggression)
                {
                    //print("NoLongerInRange");
                    enemy.SetStrikingDistanceBool(false);
                    enemy.SetRetreatDistanceBool(true);
                }
            }

        }

    }
}
