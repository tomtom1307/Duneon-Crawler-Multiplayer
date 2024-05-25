using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class EnemyStrikingDistanceCheck : MonoBehaviour
    {
        private Enemy enemy;
        private float timer;


        private void Awake()
        {
            enemy = GetComponent<Enemy>();
            

        }

        private void Update()
        {
            float distance = Vector3.Distance(enemy.target.position, transform.position) ;
            if (distance < enemy.AttackDistance)
            {
                enemy.SetStrikingDistanceBool(true);
                print("InRange");
                timer = 0;
            }
            

            else
            {
                timer += Time.deltaTime;
                if(timer > 1 - enemy.aggression)
                {
                    print("NoLongerInRange");
                    enemy.SetStrikingDistanceBool(false);
                }
            }

        }

    }
}
