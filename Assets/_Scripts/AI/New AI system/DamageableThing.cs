using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class DamageableThing : NetworkBehaviour
    {
        public Material[] origColors { get; set; }
        public Material[] whites { get; set; }
        [SerializeField] public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        [field: SerializeField] public Image HealthBar;
        [field: SerializeField] public Canvas HealthBarCanvas;
        [field: SerializeField] public Color NormalColor { get; set; }
        [field: SerializeField] public GameObject damageText;
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        MeshRenderer MR;
        public bool DoesNotDestroy;
        public GameObject DestroyVisual;
        private float flashTime = 0.1f;
        public float TimeLeftAfterDeath;
        public bool ded;
        public bool Invincible;


        private void Start()
        {
            
            ded = false;
            MR = gameObject.GetComponentInChildren<MeshRenderer>();
            origColors = MR.materials;
            whites = MR.materials;
            CurrentHealth.Value = MaxHealth;
           

        }


        [ServerRpc(RequireOwnership = false)]
        public void TakeDamageServerRpc(float Damage)
        {
            if (Invincible) return;
            CurrentHealth.Value -= Damage;
            if(CurrentHealth.Value <= 0)
            {
                ded = true;
                if (DoesNotDestroy)
                {
                    Destroy(DestroyVisual);
                }
                else
                {
                    Destroy(gameObject, TimeLeftAfterDeath);
                }
                
            }
            HandleLocalVisualsClientRpc(Damage);
        }

        [ClientRpc]
        public void HandleLocalVisualsClientRpc(float Damage)
        {

            HealthBar.fillAmount = CurrentHealth.Value / MaxHealth;


            DamageFlash();
            GenerateDamageNumber(Damage);
        }

        void DamageFlash()
        {
            FlashStart();
        }



        void FlashStart()
        {

            MR.SetMaterials(whites.ToList());



            Invoke("FlashEnd", flashTime);
        }

        void FlashEnd()
        {
            MR.SetMaterials(origColors.ToList());
        }

        void GenerateDamageNumber(float dam)
        {
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            Color color =  NormalColor;
            //indicator.SetDamageColor(color); 


            indicator.SetDamageText(Mathf.RoundToInt(dam), color);
        }

        public void ToggleInvincibility(bool toggle)
        {
            Invincible = toggle;
            HealthBarCanvas.enabled = !toggle;
        }


    }
}
