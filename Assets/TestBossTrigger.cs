using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class TestBossTrigger : NetworkBehaviour
    {

        public string BossName;
        public GameObject BossPrefab;
        public Transform BossSpawnPos;
        EnemySpawner spawner;
        RoomProgressBar RPB;
        private Boss enemy;
        private float MaxHealth;

        private void Awake()
        {
            spawner = GetComponentInChildren<EnemySpawner>();

            //Initialize Boss object in enemies struct in the spawner class 
            EnemySpawner.Enemies BossSpawner = new EnemySpawner.Enemies(name,
                BossPrefab,
                1,
                false,
                new int[] { 1 },
                new List<Transform>() { BossSpawnPos });

            //Set The boss in the spawner
            spawner.enemies.Add(BossSpawner);
        }

        private void Start()
        {
            

            
        }

        private void OnEnable()
        {
            //Subscribe to when the spawner is complete
            Actions.SpawnerUpdate += HandleSpawner;
        }

        private void OnDisable()
        {
            Actions.SpawnerUpdate -= HandleSpawner;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (IsLocalPlayer) return;
            if (other.TryGetComponent<Boss>(out enemy))
            {
                MaxHealth = enemy.MaxHealth;
            }
            if (other.gameObject.tag == "Player")
            {
                //Enable ProgressBar
                RPB = other.GetComponentInChildren<RoomProgressBar>();
                if (enemy == null) return;
                RPB.EnableProgressBar(RoomProgressBar.ProgressType.Health, BossName, Color.red);
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            //Disable ProgressBar
            if (IsLocalPlayer) return;
            if (other.gameObject.tag == "Player")
            {
                RPB = other.GetComponentInChildren<RoomProgressBar>();
                //RPB.DisableProgressBar();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                RPB = other.GetComponentInChildren<RoomProgressBar>();
                
            }
            if(enemy != null)
            {
                RPB.UpdateValue(enemy.CurrentHealth.Value / MaxHealth);
            }
            else
            {
                enemy = other.GetComponent<Boss>();
            }
        }

        public void DisableHealthBar()
        {
            if (RPB == null) return;
            RPB.DisableProgressBar();
        }
        public void EnableHealthBar()
        {
            if (RPB == null) return;
            RPB.EnableProgressBar(RoomProgressBar.ProgressType.Health, BossName, Color.red);
        }
        public void HandleSpawner(EnemySpawner ES, bool isActive)
        {
            if(ES == spawner)
            {
                if(isActive)
                {
                    EnableHealthBar();
                }
                if(!isActive)
                {
                    RPB.UpdateValue(0 / MaxHealth);
                    print("Boss Defeated!");
                    Invoke("DisableHealthBar", 1.5f);
                }
                
            }
            
        }



    }
}
