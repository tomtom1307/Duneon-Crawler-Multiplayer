using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class RoomEnemyBrain : NetworkBehaviour
    {
        public List<Transform> NestLocation;
    }
}
