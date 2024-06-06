using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class DontDestroyScript : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake() 
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
