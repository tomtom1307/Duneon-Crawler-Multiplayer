using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Project
{
    public class EnemyAi : NetworkBehaviour
    {
        public float MaxDetectDistance;
        public float Damage;
        public float AttackDistance;

        Transform target;
        private EnemyReferences enemyReferences;
        private float attackDistance;
        private float pathUpdateDeadline;
        public List<Transform> playerlist;
        Rigidbody rb;
        public Transform headTarget;

        private void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        private void Start()
        {
            attackDistance = enemyReferences.navMeshAgent.stoppingDistance + 1;
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }

            rb = GetComponent<Rigidbody>();
        }


        private void Update()
        {
            enemyReferences.animator.SetFloat("Speed", enemyReferences.navMeshAgent.velocity.magnitude/enemyReferences.navMeshAgent.speed);
            target = DetectPlayer();
            if(target != null && enemyReferences.navMeshAgent.isActiveAndEnabled)
            {
                bool inRange = (Vector3.Distance(transform.position, target.position) <= attackDistance);
                if(inRange)
                {
                    LookAtTarget();
                    if (!enemyReferences.animator.GetBool("Attacking")) Attack();

                }
                else
                {
                    UpdatePath();
                    headTarget.position = target.position;
                }
            }
        }

        private void Attack()
        {
            enemyReferences.animator.SetBool("Attacking", true);
            
        }


        public void AttackAction()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward * AttackDistance, out hit, 10);

            PlayerStats ps;
            if (hit.collider == null) return;
            hit.collider.gameObject.TryGetComponent<PlayerStats>(out ps);

            if (ps != null) ps.TakeDamage(Damage);
            
        }

        private void AttackExit()
        {
            enemyReferences.animator.SetBool("Attacking", false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position + transform.forward * AttackDistance, transform.localScale);
        }

        private Transform DetectPlayer()
        {
            float MinDist = MaxDetectDistance;
            Transform target = null;
            foreach (var item in playerlist)
            {
                if(Vector3.Distance(transform.position, item.position) < MinDist)
                {
                    MinDist = Vector3.Distance(transform.position, item.position);
                    target = item;
                }
            }

            return target;


        }

        private void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }

        


        private void UpdatePath()
        {
            if(Time.time >= pathUpdateDeadline)
            {
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
                enemyReferences.navMeshAgent.SetDestination(target.position);
            }
        }

    }
}
