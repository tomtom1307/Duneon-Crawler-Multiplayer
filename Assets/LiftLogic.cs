using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project
{
    public class LiftLogic : MonoBehaviour
    {
        Animator anim;
        public float Speed;
        public Transform StartPos;
        public Transform EndPos;
        public float maintainPlayerHeight;
        public float TargetY;
        List<Rigidbody> rb;
        public bool Moving = false;
        public bool LiftReady;
        private void Start()
        {
            LiftReady = true;
            StartPos.transform.parent = null;
            EndPos.transform.parent = null;
            rb = new List<Rigidbody>();
            TargetY = EndPos.position.y;
            anim = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!LiftReady) return;
            LiftReady = false;

            if (Moving) return;
            Moving = true;

            anim.SetFloat("UpDown", -1 * anim.GetFloat("UpDown"));
            DOTween.Sequence().SetDelay(0.6f).Append(transform.DOMoveY(TargetY, 1 / Speed).SetEase(Ease.InOutSine).OnComplete(UnParentPlayers));
            
            if (TargetY == EndPos.position.y) TargetY = StartPos.position.y;
            else TargetY = EndPos.position.y;
        }

        private void Update()
        {
            if (Moving) ParentPlayers();

        }

        public void ResetLift()
        {
            LiftReady = true;
            anim.SetFloat("UpDown", -1*anim.GetFloat("UpDown"));

        }


        private void OnCollisionEnter(Collision collision)
        {
            
            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {
                rb.Add(collision.gameObject.GetComponent<Rigidbody>());
            }
        }


        private void ParentPlayers()
        {
            foreach (var player in rb)
            {
                player.transform.position = new Vector3(player.transform.position.x, transform.position.y + maintainPlayerHeight, player.transform.position.z);
                player.isKinematic = true;
            }

        }

        private void UnParentPlayers()
        {
            Moving = false;
            if (rb.Count == 0) return;
            foreach (var player in rb)
            {
                
                player.isKinematic = false;
                
            }
            rb.Clear();
            DOTween.Sequence().SetDelay(2).OnComplete(() => ResetLift());




        }

    }
}
