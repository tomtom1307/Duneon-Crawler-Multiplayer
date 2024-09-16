using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class DamageArrowUI : MonoBehaviour
    {
        public Vector3 DamageLocation;
        public Transform PlayerOrientation;
        public Transform DamageImagePivot;


        public CanvasGroup alphaCanvasGroup;
        public float FadeStartTime, FadeTime;
        float maxFadeTime;


        private void Start()
        {
            maxFadeTime = FadeTime;
        }


        private void Update()
        {
            if (FadeStartTime > 0)
            {
                FadeStartTime -= Time.deltaTime;
            }
            else
            {
                FadeTime -= Time.deltaTime;
                alphaCanvasGroup.alpha = FadeTime / maxFadeTime;
                if(FadeTime <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
            DamageLocation.y = PlayerOrientation.position.y;
            Vector3 Direction = (DamageLocation - PlayerOrientation.position).normalized;
            float angle = Vector3.SignedAngle(Direction, PlayerOrientation.forward, Vector3.up);
            DamageImagePivot.transform.localEulerAngles = new Vector3(0,0, angle);
        }
    }
}
