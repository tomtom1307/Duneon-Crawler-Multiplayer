using Project.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class VFXHandler : NetworkBehaviour
    {
        public GameObject Attack1PS;
        public Transform EffectOrigin;
        public float Speed;
        public GameObject Attack2PS;
        public LayerMask onTouch;
        Transform target; 
        public float Distance;

        public MoveToTarget VisualInfo;

        private void Start()
        {
            if (!IsOwner) return;
            Camera.main.GetComponentInChildren<WeaponHolder>().visualAttacks = this;
            print(Camera.main.GetComponentInChildren<WeaponHolder>().visualAttacks);
        }


        [ServerRpc]
        public void ProjectileServerRpc(Vector3 EndPos)
        {
            if (Attack1PS == null) return;

            GameObject visual = Instantiate(Attack1PS, transform.position-Vector3.down*0.2f+Vector3.right*0.1f, Quaternion.LookRotation(Camera.main.transform.position));
            visual.GetComponent<NetworkObject>().Spawn();
            visual.GetComponent<MoveToTarget>().target = EndPos;
            VisualInfo = visual.GetComponent<MoveToTarget>();
        }

        [ServerRpc]
        public void AOEServerRpc(float Scale)
        {
            if (Attack2PS == null) return;
            GameObject visual = Instantiate(Attack2PS, transform.position-Vector3.up*0.4f, Quaternion.Euler(new Vector3(0,0,0)));
            visual.transform.localScale = Scale* Vector3.one;
            visual.GetComponent<NetworkObject>().Spawn();

        }




        private void Update()
        {


        }
    }
}
