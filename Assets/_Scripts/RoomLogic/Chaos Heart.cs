using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Project
{
    public class ChaosHeart : NetworkBehaviour
    {
        public float DamageReduction = 0.5f;
        [HideInInspector] public DamageableThing DT;
        public GameObject BuffVFX;
        [SerializeField] public LayerMask WhatisPlayer;
        public EnemySpawner spawner;
        public bool Trigger = false;
        public Animator TentacleAnim;
        public Color HealthbarColor;
        public EnemySpawnTrap EST;
        
        private void Start()
        {
            DT = GetComponent<DamageableThing>();

        }

        private void Update()
        {
            if(DT.CurrentHealth.Value < 0.5 * DT.MaxHealth && !Trigger)
            {
                Trigger = true;



                //Do The Roar
                DT.Invincible = true;
                TentacleAnim.SetTrigger("Protect");
                spawner.TriggerNextRound();
            }
            if (DT.ded)
            {
                EST.triggers[0].TF = true;
            }
        }


    }
}
