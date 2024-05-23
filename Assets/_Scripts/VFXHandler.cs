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
            print("FakeProj");
            GameObject visual = Instantiate(Attack1PS, transform.position-Vector3.down*0.2f, Quaternion.identity);
            visual.GetComponent<NetworkObject>().Spawn();
            visual.GetComponent<MoveToTarget>().target = EndPos;
            StartCoroutine(Delete(visual));
        }

        [ServerRpc]
        public void AOEServerRpc(float Scale)
        {
            print("AOE");
            GameObject visual = Instantiate(Attack2PS, transform.position, Quaternion.identity);
            visual.GetComponent<NetworkObject>().Spawn();
            StartCoroutine(Delete(visual));
        }

        IEnumerator Delete(GameObject GO)
        {
            yield return new WaitForSeconds(5);
            GO.GetComponent<NetworkObject>().Despawn();
            Destroy(GO,1);
        }


        private void Update()
        {


        }
    }
}
