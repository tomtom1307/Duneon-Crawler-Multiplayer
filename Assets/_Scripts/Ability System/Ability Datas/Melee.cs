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
            
            if(Physics.SphereCast(Camera.main.transform.position-DetectionRadius * Camera.main.transform.forward, DetectionRadius,Camera.main.transform.forward, out hit, Range+ DetectionRadius, whatIsEnemy))
            {
                Debug.Log("Melee!");
                Enemy TD;
                fail = false;
                if (hit.collider.tag == "Head")
                {
                    TD = hit.collider.GetComponentInParent<Enemy>();

                }
                else hit.collider.gameObject.TryGetComponent<Enemy>(out TD);

                if(TD != null)
                {
                    TD.DoDamageServerRpc(Damage);
                    //TD.DisableNavMeshServerRpc();
                    TD.KnockBackServerRpc(Camera.main.transform.position, KnockBack);

                }
                
            }



        }

    }
}
