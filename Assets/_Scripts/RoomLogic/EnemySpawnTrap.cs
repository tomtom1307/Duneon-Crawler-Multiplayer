using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static UnityEditor.Progress;

namespace Project
{
    public class EnemySpawnTrap : Director
    {
        public EnemySpawner spawner;
        public DoorLogic[] activatables;
        public Levers[] activators;


        private void Start()
        {
            foreach (var Lever in activators)
            {
                Lever.lever.EST = this;
                Lever.lever.Channel = Lever.Channel;
            }
        }


        public override void OnActivate(int Channel)
        {
            foreach (var item in activatables)
            {
                if (item.OnActivateOpen && item.Channel == Channel)
                {
                    OpenDoor(item.anim);
                }
            }
        }


        private void Update()
        {
            if (spawner.AllEnemiesKilled == true)
            {
                OnCompleted();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            print("Triggered");
            //Put Condition for players inside run 
            spawner.Active = true;
            foreach (var item in activatables)
            {
                if (item.OnTriggeredClose)
                {
                    CloseDoor(item.anim);
                }
            }
        }


        public override void OnCompleted()
        {

            foreach (var item in activatables)
            {
                if (item.OnCompletedOpen)
                {
                    OpenDoor(item.anim);
                }
                
            }
            
        }


        public void OpenDoor(Animator anim)
        {
            anim.SetBool("Open", true);
            anim.SetFloat("Direction", 1f);
        }

        public void CloseDoor(Animator anim)
        {
            anim.SetBool("Open", false);
            anim.SetFloat("Direction", -1f);
        }



    }


    [Serializable]
    public struct Levers
    {
        public string Name;
        public LeverLogic lever;
        public int Channel;
    }


    [Serializable]
    public struct DoorLogic
    {
        public string name;
        public Animator anim;
        public int Channel;
        public bool OnActivateOpen;
        public bool OnTriggeredClose;
        public bool OnCompletedOpen;
    }
}
