using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Chase-CroblinHide", menuName = "Enemy Logic/ Chase Logic/ CroblinHide")]
    public class EnemyChaseCroblinHide : EnemyChaseSOBase
    {
        private float _timer;
        public float HideTime;

        public List<Transform> NestLocation;
        public Transform targetTransform;

        public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType type)
        {
            base.DoAnimationTriggerEventLogic(type);
        }

        public override void DoEnterLogic()
        {
            base.DoEnterLogic();

            // find furthest nest and head to it

            enemy.EnableNavMesh(true); //

            NestLocation = enemy.DesignatedRoom.GetComponent<RoomEnemyBrain>().NestLocation;

            float distance = 0;

            

            foreach (Transform t in NestLocation) 
            {
                float newDistance = Vector3.Distance (enemy.transform.position, t.position);
                enemy.target = t; 

                if (distance < newDistance) 
                { 
                    distance = newDistance; 
                    targetTransform = t;
                }
            }

            _timer = Random.Range(0f, 3f);

            enemy.MoveEnemy(targetTransform.position);
        }

        public override void DoExitLogic()
        {
            base.DoExitLogic();
        }

        public override void DoFrameUpdateLogic()
        {
            //base.DoFrameUpdateLogic();

            //Debug.Log(enemy.DesignatedRoom);

            if (Vector3.Distance(enemy.transform.position, targetTransform.position) < 3)
            {
                //Start timer with HideTime

                _timer += Time.deltaTime;

                if (_timer > HideTime) 
                {
                    enemy.StateMachine.ChangeState(enemy.AttackState);
                    Debug.Log(enemy.AttackState);
                }
            }
        }

        public override void DoPhysicsUpdateLogic()
        {
            base.DoPhysicsUpdateLogic();
        }

        public override void Initialize(GameObject gameObject, NavMeshEnemy enemy)
        {
            base.Initialize(gameObject, enemy);
        }

        public override void ResetValues()
        {
            base.ResetValues();
        }

    }
}
