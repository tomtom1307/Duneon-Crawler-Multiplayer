using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    [DisallowMultipleComponent]
    public class EnemyReferences : MonoBehaviour
    {
        [HideInInspector]public NavMeshAgent navMeshAgent;
        [HideInInspector]public Animator animator;
        public EnemyAi AI;
        public DissolveController DissolveController;

        [Header("Stats")]

        public float pathUpdateDelay = 0.2f;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            DissolveController = GetComponent<DissolveController>();
            animator = GetComponent<Animator>();
            AI = GetComponent<EnemyAi>();
        }

    }
}
