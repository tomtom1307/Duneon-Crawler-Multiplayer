using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;
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
        public bool SkinnedMesh;
        bool Ded;
        MeshRenderer meshRenderer;
        SkinnedMeshRenderer SkinmeshRenderer;
        [SerializeField] Material[] origColors;
        Material[] whites;
        EnemyReferences ER;
        Camera cam;
        Rigidbody rb;
        public List<Collider> colliders;
        

        private void Start()
        {
            
            
            Ded = false;
            rb = GetComponent<Rigidbody>();
            if(GetComponent<EnemyReferences>() != null )
            {
                ER = GetComponent<EnemyReferences>();
            }
            
            
            if(SkinnedMesh)
            {
                SkinmeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                origColors = SkinmeshRenderer.materials;
                whites = SkinmeshRenderer.materials;

            }
            else
            {
                meshRenderer = gameObject.GetComponent<MeshRenderer>();
                origColors = meshRenderer.materials;
                whites = meshRenderer.materials;
            }
            cam = Camera.main;
            
            for (int i = 0; i < origColors.Length; i++)
            {
                whites[i] = white;

            }



            if (NetworkManager.Singleton.LocalClientId == 0)
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

        [ClientRpc]
        void DisableHealthBarClientRpc()
        {
            

            HealthCanvas.enabled = false;
            ER.AI.enabled = false;

            
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
            if (SkinnedMesh)
            {
                SkinmeshRenderer.SetMaterials(whites.ToList());
            }
            else
            {
                meshRenderer.SetMaterials(whites.ToList());
            }


            Invoke("FlashEnd", flashTime);
        }

        void FlashEnd()
        {

            if (SkinnedMesh)
            {
                SkinmeshRenderer.SetMaterials(origColors.ToList());
                
            }
            else
            {
                meshRenderer.SetMaterials(origColors.ToList());
            }
            
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNavMeshServerRpc()
        {
            ER.navMeshAgent.enabled = true;
            ER.animator.applyRootMotion = true;
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisableNavMeshServerRpc()
        {
            if (!SkinnedMesh || Ded) return;

            

            if (_health.Value <= 0 )
            {
                Ded = true;
                ER.animator.Play("Die", -1, 0);
                ER.DissolveController.StartDissolve();
                DisableHealthBarClientRpc();
                

               
                Destroy(ER.navMeshAgent);
                CancelInvoke("EnableNavMeshServerRpc");
                Invoke("Delete", 4);
                foreach (var item in colliders)
                {
                    item.enabled = false;
                }
                Destroy(GetComponent<NetworkRigidbody>());
                Destroy(rb);

            }
            else
            {
                
                ER.navMeshAgent.enabled = false;
                ER.animator.Play("Hit", -1, 0f);
                ER.animator.applyRootMotion = false;
                Invoke("EnableNavMeshServerRpc", 1f);
            }
            
        }

        public void Delete()
        {
            Destroy(gameObject);
        }



    }
}
