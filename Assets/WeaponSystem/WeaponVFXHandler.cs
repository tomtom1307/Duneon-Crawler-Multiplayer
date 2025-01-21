using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class WeaponVFXHandler : MonoBehaviour
    {



        public GameObject Attack1HitParticleFX;
        public GameObject Attack2HitParticleFX;
        public GameObject IdleParticles;
        public GameObject ChargeParticles;



        public List<GameObject> AttackVFX;

        public List<VFXSpawn> vFXSpawns;

        public struct VFXSpawn
        {
            public Vector3 pos;
            public Vector3 rot;
            public Vector3 Scale;
            public GameObject VFX;
        }

        //Modularly swapped particles
        public void SpawnOnHitParticleFX(Transform t)
        {
            if(Attack1HitParticleFX == null) { return; }
            GameObject part = Instantiate(Attack1HitParticleFX, t.position, Quaternion.identity);
            part.layer = 1;
        }


        public void SpawnOnIdleParticleFX()
        {
            if (Attack1HitParticleFX == null) { return; }
        }

        public void SpawnOnChargeParticleFX()
        {
            if (Attack1HitParticleFX == null) { return; }
        }





        // Generic 1 time use implementations 
        public void SpawnVFXOnWeapon()
        {

        }

        public void SpawnListAttackVFX_Parented(Transform t, int i)
        {

        }

        // Generic 1 time use implementations 
        public void SpawnListAttackVFX(Transform t, int i)
        {
            GameObject part = Instantiate(AttackVFX[i], t.position, Quaternion.identity);
        }




    }
}
