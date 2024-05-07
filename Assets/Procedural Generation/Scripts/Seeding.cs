using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Project
{

    
    public class Seeding : NetworkBehaviour 
    {
    
        private int CurrentSeed;
        public NetworkVariable<int> GeneratedSeed = new NetworkVariable<int>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        private void Awake() 
            {
                CurrentSeed = Random.Range(0, 100000000);
                Random.InitState(CurrentSeed);
                GeneratedSeed.Value = CurrentSeed;
            }
    }
}
