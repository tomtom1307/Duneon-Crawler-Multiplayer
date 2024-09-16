using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class DontDestroyNetwork : NetworkBehaviour
    {
        // Start is called before the first frame update
        private void Awake() 
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
