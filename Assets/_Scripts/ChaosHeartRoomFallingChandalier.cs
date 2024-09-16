using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ChaosHeartRoomFallingChandalier : MonoBehaviour
    {
        [HideInInspector]public DamageableThing DT;
        public ChaosHeart CH;
        public Animator TentacleAnim;
        Animator anim;


        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            DT = GetComponentInChildren<DamageableThing>();
            DT.ToggleInvincibility(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (DT.ded)
            {
                CH.DT.ToggleInvincibility(false);
                anim.SetTrigger("Fall");
                TentacleAnim.SetTrigger("Hide");
            }
        }
    }
}
