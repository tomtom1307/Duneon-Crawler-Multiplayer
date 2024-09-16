using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class GenerateButton : MonoBehaviour
    {
        private Seeding Generator;
        private void Awake()
        {
            Generator = GetComponent<Seeding>();
        }

        public void DoStuff()
        {
            Generator.HostGen();
        }


    }
}
