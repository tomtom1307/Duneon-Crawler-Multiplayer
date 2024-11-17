using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(DissolveController))]
    [RequireComponent(typeof(Animator))]
    public class NavMeshEnemy : Enemy
    {
        public bool DisableOnStart;
        [Header("Additional Enemy States")]
        [SerializeField] private EnemyChaseSOBase EnemyChaseBase;

        public EnemyChaseState ChaseState { get; set; }
        public NavMeshAgent navMesh { get; set; }


        private float staggerRegenSpeed;
        bool StaggerRegen = false;


        public override void Awake()
        {
            base.Awake();
            EnemyChaseInstance = Instantiate(EnemyChaseBase);
            ChaseState = new EnemyChaseState(this, StateMachine);
        }


        // Start is called before the first frame update
        public override void Start()
        {
            
            navMesh = GetComponent<NavMeshAgent>();
            if (DisableOnStart) {
                gameObject.SetActive(false);
            }
            base.Start();
            print(navMesh);
            navMesh.avoidancePriority = Random.Range(0, 50);
            rb.isKinematic = true;
            rb.freezeRotation = true;
            

            SkinmeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            if (SkinmeshRenderer == null)
            {
                Debug.LogError("This Script Does not support non static enemy types without a skinnedMeshRenderer");
            }
            origColors = SkinmeshRenderer.materials;
            whites = SkinmeshRenderer.materials;

        }

        public override void Update()
        {
            Debug.DrawRay(transform.position + 0.5f * Vector3.up, transform.forward * 2f);
            StateMachine.currentState.FrameUpdate();
            if (navMesh == null) return;
            animator.SetFloat("SpeedX", Vector3.Dot(navMesh.velocity, transform.right));
            animator.SetFloat("SpeedY", Vector3.Dot(navMesh.velocity, transform.forward));

            if (StaggerRegen && CurrentStaggerHealth.Value < MaxStaggerHealth)
            {
                CurrentStaggerHealth.Value += staggerRegenSpeed * Time.deltaTime;

            }
        }
        public void RegenStaggerHealth()
        {
            StaggerRegen = true;
        }


        [HideInInspector] public bool Floating;
        [HideInInspector] public float floatHeight;

        float floatattackDamage;

        public void MoveEnemy(Vector3 targetPosition)
        {
            if (!navMesh.enabled) return;


            navMesh.SetDestination(targetPosition);
        }


        [ServerRpc(RequireOwnership = false)]
        public void EnableNavMeshServerRpc()
        {
            
            if (rb == null) return;
            rb.isKinematic = true;
            navMesh.enabled = true;
            animator.applyRootMotion = true;

        }




        [ServerRpc(RequireOwnership = false)]
        public void FloatAttackRecieveServerRpc(float Height, float Duration, float Damage)
        {

            CancelInvoke("EnableNavMeshServerRpc");
            Floating = true;
            floatHeight = Height;
            rb.isKinematic = true;
            navMesh.enabled = false;
            transform.DOMove(Height * Vector3.up + transform.position, 0.5f);
            floatattackDamage = Damage;
            animator.applyRootMotion = false;
            animator.Play("Floating", -1, 0);

            //print("Floating!");
            Invoke("EndFloatingEffectServerRpc", Duration);

        }

        [ServerRpc(RequireOwnership = false)]
        private void EndFloatingEffectServerRpc()
        {
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.LevitationHit, 0.7f);
            transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.2f);
            DoDamageServerRpc(floatattackDamage);


            rb.isKinematic = false;
            animator.applyRootMotion = true;
            animator.Play("Hit", -1, 0);
            Invoke("EnableNavMeshServerRpc", 0.5f);
            Floating = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisableNavMeshServerRpc(bool DisableAutoReenable = false)
        {

            if (Floating) return;

            else if (StateMachine.currentState == DedState)
            {
                return;
            }
            else
            {

                navMesh.enabled = false;
                //animator.Play("Hit", -1, 0f);
                animator.applyRootMotion = false;
                if (DisableAutoReenable) return;
                Invoke("EnableNavMeshServerRpc", 1f);
            }

        }

        public override void InitializeStateMachine()
        {
            base.InitializeStateMachine();
            EnemyChaseInstance.Initialize(gameObject, this);


            StateMachine.Initialize(ChaseState);
        }

        public override void Die()
        {
            navMesh.enabled = false;
            RemoveArmorBuff();
            //Invoke("Delete", 4);
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
            CancelInvoke("EnableNavMeshServerRpc");

            if (Floating)
            {
                transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.1f);
                animator.Play("FallingDeath", -1, 0);
                CancelInvoke("EndFloatingEffectServerRpc");
            }
            else animator.Play("Die", -1, 0);

            dissolve.StartDissolveClientRpc();

            Destroy(gameObject,4);

            Destroy(GetComponent<NetworkRigidbody>());
            rb.isKinematic = true;

            
            base.Die();
        }

        [ServerRpc(RequireOwnership = false)]
        public override void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            base.DoDamageServerRpc(Damage, headshot);
            if (CurrentHealth.Value <= 0)
            {
                StateMachine.ChangeState(DedState);
                DisableNavMeshServerRpc();
                return;
            }

            CurrentStaggerHealth.Value -= (Damage);

            if (CurrentStaggerHealth.Value < 0)
            {
                DisableNavMeshServerRpc();
                CurrentStaggerHealth.Value = MaxStaggerHealth;
                animator.Play("Hit", -1, 0f);
            }

            
        }


        public override void FlashStart()
        {
            SkinmeshRenderer.SetMaterials(whites.ToList());
        }

        public override void FlashEnd()
        {
            SkinmeshRenderer.SetMaterials(origColors.ToList());
        }

        
    }
}
