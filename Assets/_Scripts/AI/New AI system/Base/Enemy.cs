using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Project
{
    public class Enemy : NetworkBehaviour, IDamageable, IEnemyMoveable, ITriggerCheckable
    {
        #region Definitions
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public NetworkVariable<float> CurrentHealth { get; set; } = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        [field: SerializeField] public float maxDetectDist { get; set; } = 100f;
        [field: SerializeField] public float AttackDistance { get; set; } = 4f;
        [field: SerializeField] public float lungeDistance { get; set; } = 4f;
        [field: SerializeField] public float TimeBetweenAttacks { get; set; } = 4f;
        [field: SerializeField] public float AttackDamage { get; set; } = 4f;
        [field: SerializeField] public Image  HealthBar;
        [field: SerializeField] public int xpOnKill { get; set; }
        [field: SerializeField] public LayerMask whatisPlayer{ get; set; }
        public Rigidbody rb { get; set ; }
        public NavMeshAgent navMesh { get; set; }

        public List<Collider> colliders;
        public Canvas HealthCanvas;

        public Animator animator { get; set; }
        
        public List<Transform> playerlist;
        [HideInInspector] public Transform target;
        public float aggression = 0.5f;

        public EnemyStateMachine StateMachine { get; set; }
        public EnemyIdleState IdleState { get; set; }
        public EnemyChaseState ChaseState { get; set; }
        public EnemyAttackState AttackState { get; set; }
        public EnemyDedState DedState{ get; set; }
        public bool IsWithinStrikingDistance { get; set; }
        public bool PlayerIsTooClose { get; set; }
        [field: SerializeField] public Color HeadshotColor { get; set; }
        [field: SerializeField] public Color NormalColor { get; set; }
        public Material[] origColors { get; set; }
        public Material[] whites { get; set; }
        public float flashTime { get; set; } = 1f;
        public DissolveController dissolve { get; set; }

        public float RandomMovementRange = 5f;
        public float RandomMovementSpeed = 1f;

        //VFX Stuff


        SkinnedMeshRenderer SkinmeshRenderer;
        [field: SerializeField] public GameObject damageText;


        #endregion

        private void Awake()
        {
            //Contruct StateMachine
            StateMachine = new EnemyStateMachine();


            //Contruct States
            IdleState = new EnemyIdleState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            DedState = new EnemyDedState(this, StateMachine);
        }

        private void Start()
        {
            
            //Set MaxHealth
            CurrentHealth.Value = MaxHealth;
            //Get References
            rb = GetComponent<Rigidbody>(); 
            navMesh = GetComponent<NavMeshAgent>();
            
            animator = GetComponent<Animator>();

            //Find Players in lobby
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }

            //Initialize StateMachine
            StateMachine.Initialize(ChaseState);

            //Final Setup shit
            rb.isKinematic = true;
            dissolve = GetComponent<DissolveController>();
            //
            SkinmeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            if (SkinmeshRenderer == null)
            {
                Debug.LogError("This Script Does not support enemy types without a skinnedMeshRenderer");
                Destroy(this);
            }
            origColors = SkinmeshRenderer.materials;
            whites = SkinmeshRenderer.materials;
        }
        
        #region Health/Networking

        [ServerRpc(RequireOwnership = false)]
        public void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            if (headshot) Damage *= 2;
            CurrentHealth.Value -= Damage;
            aggression += 0.05f;
            HandleLocalVisualsClientRpc(Damage, headshot);

        }



        [ServerRpc(RequireOwnership = false)]
        public void KnockBackServerRpc(Vector3 playerPos, float KnockBack = 5)
        {
            rb.isKinematic = false;
            Vector3 dir = transform.position - playerPos;
            rb.AddForce(dir.normalized * KnockBack, ForceMode.Force);
        }


        [HideInInspector] public bool Floating;
        [HideInInspector] public float floatHeight;

        [ServerRpc(RequireOwnership = false)]
        public void FloatAttackRecieveServerRpc(float Height, float Duration)
        {
            CancelInvoke("EnableNavMeshServerRpc");
            Floating = true;
            floatHeight = Height;
            rb.isKinematic = true;
            transform.DOMove(Height * Vector3.up + transform.position, 0.5f);
            
            navMesh.enabled = false;
            animator.applyRootMotion = false;
            animator.Play("Floating", -1, 0);

            print("Floating!");
            Invoke("EndFloatingEffectServerRpc", Duration);

        }

        [ServerRpc(RequireOwnership = false)]
        private void EndFloatingEffectServerRpc()
        {
            transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.2f);
            DoDamageServerRpc(1);
            animator.Play("Hit", -1, 0f);
            rb.isKinematic = false;
            animator.applyRootMotion = true;
            animator.Play("Movement");
            Invoke("EnableNavMeshServerRpc", 0.5f);
            Floating = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisableNavMeshServerRpc()
        {
            if (CurrentHealth.Value <= 0)
            {

                StateMachine.ChangeState(DedState);


            }
            else if (Floating) return;
            else
            {

                navMesh.enabled = false;
                animator.Play("Hit", -1, 0f);
                animator.applyRootMotion = false;
                Invoke("EnableNavMeshServerRpc", 1f);
            }

        }

        public void Die()
        {
            Destroy(navMesh);
            //TODO IMPLIMENT DED STATE
            OnDeathTellSpawner();
            if (Floating)
            {
                transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.1f);
                animator.Play("FallingDeath", -1, 0);
                CancelInvoke("EndFloatingEffectServerRpc");
            }
            else animator.Play("Die", -1, 0);

            dissolve.StartDissolveClientRpc();
            DisableHealthBarClientRpc();
            GameManager.instance.AwardXPServerRpc(xpOnKill);



            CancelInvoke("EnableNavMeshServerRpc");
            Invoke("Delete", 4);
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
            Destroy(GetComponent<NetworkRigidbody>());
            Destroy(rb);
            Destroy(gameObject, 5);

        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNavMeshServerRpc()
        {
            rb.isKinematic = true;
            navMesh.enabled = true;
            animator.applyRootMotion = true;

        }

        [ClientRpc]
        void DisableHealthBarClientRpc()
        {
            HealthCanvas.enabled = false;
        }

        [HideInInspector] public EnemySpawner Spawner;
        public virtual void OnDeathTellSpawner()
        {
            if (Spawner != null)
            {
                Spawner.EnemiesLeft -= 1;
            }
        }


        #region DamageVFX
        
        [ClientRpc]
        public void HandleLocalVisualsClientRpc(float Damage, bool headshot = false)
        {

            HealthBar.fillAmount = CurrentHealth.Value / MaxHealth;


            DamageFlash();
            GenerateDamageNumber(Damage, headshot);
        }

        


        void GenerateDamageNumber(float dam, bool headshot = false)
        {
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            Color color = headshot ? HeadshotColor : NormalColor;
            //indicator.SetDamageColor(color); 


            indicator.SetDamageText(Mathf.RoundToInt(dam), color);
        }

        #region Damage Flash

        void DamageFlash()
        {
            FlashStart();
        }



        void FlashStart()
        {

            SkinmeshRenderer.SetMaterials(whites.ToList());



            Invoke("FlashEnd", flashTime);
        }

        void FlashEnd()
        {
            SkinmeshRenderer.SetMaterials(origColors.ToList());
        }

        #endregion

        #endregion






        #endregion

        public void Attack()
        {

            RaycastHit hit;
           
            Physics.SphereCast(transform.position+0.5f*Vector3.up, 0.2f, transform.forward, out hit, 0.7f, whatisPlayer);
            if (hit.collider != null)
            {

                hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(AttackDamage,transform.position);
            }


        }


        public void MoveEnemy(Vector3 targetPosition)
        {
            if (!navMesh.enabled) return;
            navMesh.SetDestination(targetPosition);
        }


        
        public virtual void AttackExit()
        {

            animator.SetBool("Attacking", false);
        }


        public void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        }


        private void Update()
        {
            if(!IsOwner) return;
            Debug.DrawRay(transform.position + 0.5f * Vector3.up, transform.forward * 2f);
            StateMachine.currentState.FrameUpdate();
            if (navMesh == null) return;
            animator.SetFloat("SpeedX", Vector3.Dot(navMesh.velocity, transform.right));
            animator.SetFloat("SpeedY", Vector3.Dot(navMesh.velocity, transform.forward));
        }

        private void FixedUpdate()
        {
            if(!IsOwner) return;
            StateMachine.currentState.PhysicsUpdate();
        }

        public Transform DetectPlayer()
        {
            float MinDist = maxDetectDist;
            Transform target = null;
            foreach (var item in playerlist)
            {
                if (Vector3.Distance(transform.position, item.position) < MinDist)
                {
                    MinDist = Vector3.Distance(transform.position, item.position);
                    target = item;
                }
            }


            return target;
        }



        #region Animation Triggers

        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            if (!IsOwner) return;
            StateMachine.currentState.AnimationTriggerEvent(triggerType);
        }

        public void SetStrikingDistanceBool(bool isStriking)
        {
            IsWithinStrikingDistance = isStriking;
        }

        public void SetRetreatDistanceBool(bool Close)
        {
            PlayerIsTooClose = Close;
        }




        public enum AnimationTriggerType
        {
            CallHit,
            FinishedAttacking,
            EnemyDamaged,
            PlayFootStepSound
        }


        #endregion

    }
}
