using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class TakeDamage : NetworkBehaviour
    {
        
        [Header("Health")]
        public float Health;
        public float MaxHealth = 100000;
        public Image HealthBar;
        public Canvas HealthCanvas;
        public NetworkVariable<float> _health = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        [Header("Damage Flash")]
        public float flashTime = 1f;
        public KeyCode DebugButton;
        public Material white;
        public Color HeadshotColor;
        public Color NormalColor;
        public GameObject damageText;


        MeshRenderer meshRenderer;
        Material[] origColors;
        Material[] whites;
        Camera cam;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            cam = Camera.main;
            origColors = meshRenderer.materials;
            whites = meshRenderer.materials;
            for (int i = 0; i < origColors.Length; i++)
            {
                whites[i] = white;

            }



            if(NetworkManager.Singleton.LocalClientId == 0)
            {
                _health.Value = MaxHealth;
            }

            

        }

        



        private void Update()
        {
            HealthBar.fillAmount = _health.Value / MaxHealth;



            //_health.Value = Health;
            FaceUIToPlayer();
            
        }


        [ServerRpc(RequireOwnership = false)]
        public void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            if (headshot) Damage *= 2;
            _health.Value -= Damage;
            
            HandleLocalVisualsClientRpc(Damage, headshot);
            
        }


        [ClientRpc]
        public void HandleLocalVisualsClientRpc(float Damage, bool headshot = false)
        {

            HealthBar.fillAmount = _health.Value / MaxHealth;
            DamageFlash();
            GenerateDamageNumber(Damage, headshot);
        }


        [ServerRpc(RequireOwnership = false)]
        public void KnockBackServerRpc(Vector3 playerPos, float KnockBack = 5)
        {
            Vector3 dir = transform.position - playerPos;
            rb.AddForce(dir.normalized*KnockBack, ForceMode.Force);
        }


        void GenerateDamageNumber(float dam, bool headshot = false)
        {
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            Color color  = headshot ? HeadshotColor : NormalColor;
            //indicator.SetDamageColor(color); 


            indicator.SetDamageText(Mathf.RoundToInt(dam), color);
        }

        void FaceUIToPlayer()
        {
            HealthCanvas.transform.LookAt(cam.transform.position);
        }

        void DamageFlash()
        {
            FlashStart();
        }

        void FlashStart()
        {

            meshRenderer.SetMaterials(whites.ToList());

            Invoke("FlashEnd", flashTime);
        }

        void FlashEnd()
        {

            meshRenderer.SetMaterials(origColors.ToList());
        }



    }
}
