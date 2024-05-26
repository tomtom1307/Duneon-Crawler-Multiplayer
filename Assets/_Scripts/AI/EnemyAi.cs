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
        public EnemySpawner Spawner;
        Transform target;
        private EnemyReferences enemyReferences;
        private float attackDistance;
        private float pathUpdateDeadline;
        
        public List<Transform> playerlist;
        Rigidbody rb;
        public Transform headTarget;

        public virtual void Awake()
        {
            enemyReferences = GetComponent<EnemyReferences>();
        }

        public virtual void Start()
        {
            attackDistance = enemyReferences.navMeshAgent.stoppingDistance + 1;
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }
            
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }


        public virtual void Update()
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

        public virtual void Attack()
        {
            enemyReferences.animator.SetBool("Attacking", true);
            
        }


        public virtual void AttackAction()
        {
            PlayerStats ps = null;
            
            Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(10,10,10), Quaternion.identity, 10);
            
            foreach (var hit in hits)
            {
                if (hit.GetComponent<Collider>() == null) return;
                hit.GetComponent<Collider>().gameObject.TryGetComponent<PlayerStats>(out ps);
                if (ps != null) ps.TakeDamage(Damage,transform.position);
            }
            
            

            
            
        }

        public virtual void AttackExit()
        {
            enemyReferences.animator.SetBool("Attacking", false);
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position + transform.forward * AttackDistance, transform.localScale);
        }

        public virtual Transform DetectPlayer()
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

        public virtual void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }




        public virtual void UpdatePath()
        {
            if(Time.time >= pathUpdateDeadline)
            {
                pathUpdateDeadline = Time.time + enemyReferences.pathUpdateDelay;
                enemyReferences.navMeshAgent.SetDestination(target.position);
            }
        }

        public virtual void OnDeathTellSpawner()
        {
            if(Spawner != null)
            {
                Spawner.EnemiesLeft -= 1;
            }
        }


    }
}
