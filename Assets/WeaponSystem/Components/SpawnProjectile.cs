using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Project
{
    public class SpawnProjectile : WeaponComponent<ProjectileSpawnData, AttackProjectileSpawn>
    {
        Camera cam;
        float ChargePercentage;
        PlayerAttack PA;

        protected override void Start()
        {
            base.Start();
            eventHandler.OnProjectileAction += SpawnPlayerProjectile;
            cam = Camera.main;
            PA = GetComponent<PlayerAttack>();
        }

        public void SpawnPlayerProjectile()
        {
            ChargePercentage = PA.ChargePercentage;
            int spawnTarget = Mathf.RoundToInt((data.SpawnAmount * ChargePercentage));
            for (int i = 0; i < spawnTarget; i++) {
                float manaUse;
                if (!weapon.statManager.stats.DoMagicAttack(weapon.Data.Attack2ManaUse, false))
                {
                    print("No Projectile Mana is too Low ");

                    return;
                }

                else manaUse = weapon.Data.Attack2ManaUse;
                GameObject proj = Instantiate(currentAttackData.Projectile, cam.transform.position, Quaternion.identity);
                NetworkObject NO = proj.GetComponent<NetworkObject>();
                NO.Spawn();
                PlayerProjectile PP = proj.GetComponent<PlayerProjectile>();
                PP.Speed = currentAttackData.Speed;
                PP.damage = currentAttackData.Damage*ChargePercentage;
                weapon.statManager.stats.DoMagicAttack(manaUse);
                Vector3 dir = (cam.transform.forward) + new Vector3(Random.Range(-currentAttackData.Deviation, currentAttackData.Deviation), Random.Range(-currentAttackData.Deviation, currentAttackData.Deviation), Random.Range(-currentAttackData.Deviation, currentAttackData.Deviation));
                PP.Direction = dir.normalized;


            }


        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            eventHandler.OnProjectileAction -= SpawnPlayerProjectile;
        }

    }
}
