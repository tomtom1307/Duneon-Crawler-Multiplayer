using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    public interface IEnemyMoveable
    {
        Rigidbody rb { get; set; }
        NavMeshAgent navMesh {  get; set; }
        void MoveEnemy(Vector3 targetPosition);
    }
}
