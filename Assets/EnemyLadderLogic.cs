using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    public class EnemyLadderLogic : MonoBehaviour
    {
        public List<Enemy> enemies;
        public OffMeshLink offMeshLink;


        private void OnTriggerEnter(Collider other)
        {
            NavMeshEnemy enemy;

            if (other.gameObject.TryGetComponent<NavMeshEnemy>(out enemy))
            {
                print("Enemy Ladder Climb");

                if (Vector3.Distance(enemy.navMesh.transform.position, offMeshLink.endTransform.position) < 2)
                {
                    //enemy.enabled = false;
                    //enemy.DisableNavMeshServerRpc();
                    print("Entered Ladder from Top");
                    DOTween.Sequence().Append(enemy.navMesh.transform.DOMove(new Vector3(offMeshLink.startTransform.position.x, offMeshLink.endTransform.position.y, offMeshLink.startTransform.position.z), 0.7f)).Append(enemy.navMesh.transform.DOMove(offMeshLink.startTransform.position, 4f)).Append(enemy.navMesh.transform.DOMove(offMeshLink.startTransform.position, .3f)).OnComplete(() => AIExitLadder(enemy));
                }
                else
                {
                    print("Entered Ladder from bottom");
                    //enemy.DisableNavMeshServerRpc();
                    DOTween.Sequence().Append(enemy.navMesh.transform.DOMove(offMeshLink.startTransform.position, 0.7f)).Append(enemy.navMesh.transform.DOMove(new Vector3(offMeshLink.startTransform.position.x, offMeshLink.endTransform.position.y, offMeshLink.startTransform.position.z), 4f)).Append(enemy.navMesh.transform.DOMove(offMeshLink.endTransform.position, .3f)).OnComplete(() => AIExitLadder(enemy));
                }
            }
        }
        public void AIExitLadder(NavMeshEnemy enemy)
        {
            enemy.enabled = true;
            enemy.EnableNavMeshServerRpc();
        }

    }
}
