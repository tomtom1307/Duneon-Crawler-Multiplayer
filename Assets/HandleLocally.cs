using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class HandleLocally : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (!IsOwner)
            {
                Destroy(gameObject);
            }
        }

    }
}
