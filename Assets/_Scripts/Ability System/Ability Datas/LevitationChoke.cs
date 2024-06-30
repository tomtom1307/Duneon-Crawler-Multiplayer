using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(fileName = "LevitationChoke", menuName = "Abilities/LevitationChoke")]
    public class LevitationChoke : Ability
    {
        public float Height;
        public float Range;
        public float ManaUse;
        public float Damage;
        public LayerMask whatIsEnemy;
        PlayerMovement PM;
        public PlayerSoundSource.SoundType HitSound;
        int Counter = 0;

        public override void Activate(GameObject parent, out bool fail)
        {
            if (parent.GetComponent<PlayerStats>()._mana.Value < ManaUse)
            {
                fail = true;
                return;
            }
            base.Activate(parent, out fail);
            
            List<Collider> EnemiesFound = Physics.OverlapSphere(parent.transform.position, Range, whatIsEnemy).ToList();
            Enemy TD;
            parent.GetComponent<PlayerStats>()._mana.Value -= ManaUse;
            Counter = 0;
            foreach (Collider c in EnemiesFound)
            {

                if (c.gameObject.TryGetComponent<Enemy>(out TD))
                {
                    
                    Counter++;

                    TD.FloatAttackRecieveServerRpc(Height, activeTime, Damage);
                }

            }

        }




    }
}
