using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Abilities/Melee")]
    public class Melee : Ability
    {
        public float Damage;
        public float Range;
        public float KnockBack;
        public float DetectionRadius;
        public float Speed;
        public LayerMask whatIsEnemy;
        public override void Activate(GameObject parent, out bool fail)
        {

            base.Activate(parent, out fail);

            RaycastHit hit;
            Debug.Log(Physics.SphereCast(Camera.main.transform.position, DetectionRadius, Camera.main.transform.forward, out hit, Range, whatIsEnemy));
            if(Physics.SphereCast(Camera.main.transform.position, DetectionRadius, Camera.main.transform.forward, out hit, Range, whatIsEnemy))
            {
                Enemy TD;
                fail = false;
                if (hit.collider.gameObject.TryGetComponent<Enemy>(out TD))
                {

                    TD.DoDamageServerRpc(Damage);
                    TD.DisableNavMeshServerRpc();
                    if (hit.collider.GetComponent<Rigidbody>() != null)
                    {

                        TD.KnockBackServerRpc(Camera.main.transform.position, KnockBack);
                    }
                }
            }



        }

    }
}
