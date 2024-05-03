using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class Timer
    {

        public event Action OntimerDone;

        private float startTime;
        private float Duration;
        private float targetTime;

        private bool isActive;

        public Timer(float duration)
        {
            this.Duration = duration;
        }

        public void StartTimer()
        {
            startTime = Time.time;
            targetTime = startTime + Duration;
            isActive = true;
        }

        public void StopTimer()
        {
            isActive = false;
        }

        public void Tick() 
        {
            if(!isActive) { return; }

            if(Time.time >= targetTime)
            {
                OntimerDone?.Invoke();
                StopTimer();

            }
        }


    }
}
