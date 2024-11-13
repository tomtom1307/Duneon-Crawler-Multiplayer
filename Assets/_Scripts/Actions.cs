using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project
{
    public static class Actions
    {

        public static Action<EnemySpawner, bool> SpawnerUpdate;
        public static Action PlayerStart;
        public static Action SpawnerStarted;
        public static Action InitializeMultiplayerStuffinScene;
        public static void Initialize() {
            Debug.Log("Initialized Actions");
        }


        // Chaos Heart Room 
        public static Action TriggerShieldGenerator;
        public static Action<ShieldGeneratorTentacle, bool> ShieldGeneratorTentacleUpdate;

    }
}
