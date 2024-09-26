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
        
        GameObject Visual;

        private void Start()
        {
            if (!IsOwner) return;
            Camera.main.GetComponentInChildren<WeaponHolder>().visualAttacks = this;
            print(Camera.main.GetComponentInChildren<WeaponHolder>().visualAttacks);
        }


        [ServerRpc]
        public void FakeProjectileServerRpc(Vector3 EndPos)
        {

            GameObject visual = Instantiate(Attack1PS, transform.position-Vector3.down*0.2f, Quaternion.identity);
            visual.GetComponent<NetworkObject>().Spawn();
            visual.GetComponent<MoveToTarget>().target = EndPos;
        }

        [ServerRpc]
        public void AOEServerRpc(float Scale)
        {
            GameObject visual = Instantiate(Attack2PS, transform.position-Vector3.up*0.4f, Quaternion.Euler(new Vector3(-90,0,0)));
            visual.transform.localScale = Scale* Vector3.one;
            visual.GetComponent<NetworkObject>().Spawn();

        }




        private void Update()
        {


        }
    }
}
