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
        public bool StaticEnemy;
        [HideInInspector] public float DamageReduction = 1;

        public float MaxStaggerHealth;
        public NetworkVariable<float> CurrentStaggerHealth { get; set; } = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);

        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public NetworkVariable<float> CurrentHealth { get; set; } = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        [field: SerializeField] public float maxDetectDist { get; set; } = 100f;
        [field: SerializeField] public float AttackDistance { get; set; } = 4f;
        [field: SerializeField] public float MeleeRaycastRange { get; set; } = 0.7f;

      
        [field: SerializeField] public float AttackDamage { get; set; } = 4f;
        [field: SerializeField] public Image  HealthBar;
        [field: SerializeField] public int xpOnKill { get; set; }
        [field: SerializeField] public LayerMask whatisPlayer{ get; set; }
        public Rigidbody rb { get; set ; }
        public NavMeshAgent navMesh { get; set; }

        public List<Collider> colliders;
        public Canvas HealthCanvas;
        public Transform ProjectileSpawnPos;
        public Animator animator { get; set; }
        
        [HideInInspector]public List<Transform> playerlist;
        [HideInInspector] public Transform target;
        [HideInInspector] public float aggression = 0.5f;
        public EnemyStateMachine StateMachine { get; set; }
        public EnemyIdleState IdleState { get; set; }
        public EnemyChaseState ChaseState { get; set; }
        public EnemyAttackState AttackState { get; set; }
        public EnemyDedState DedState{ get; set; }
        public bool IsWithinStrikingDistance { get; set; }
        public bool PlayerIsTooClose { get; set; }
        [field: SerializeField] public Color HeadshotColor = Color.HSVToRGB(0,7.6f,1.00f);
        [field: SerializeField] public Color NormalColor = Color.HSVToRGB(0, 0, 1.00f);
        public Material[] origColors { get; set; }
        public Material[] whites { get; set; }
        public float flashTime { get; set; } = 1f;
        public DissolveController dissolve { get; set; }

        
        public GameObject SpawnedObj;
        public float RotateSpeed = 100f;
        //VFX Stuff
        SkinnedMeshRenderer SkinmeshRenderer;
        [field: SerializeField] public GameObject damageText;
        public bool Attacking;

        #endregion

        #region ScriptableObject Variables

        [SerializeField] private EnemyChaseSOBase EnemyChaseBase;
        [SerializeField] private EnemyAttackSOBase EnemyAttackBase;

        public int ChoasCoresOnDeath;
        public GameObject ChaosCores;
        public EnemyChaseSOBase EnemyChaseInstance { get; set; }
        public EnemyAttackSOBase EnemyAttackInstance { get; set; }

        MeshRenderer MR;

        private float staggerRegenSpeed;
        bool StaggerRegen = false;


        #endregion



        private void Awake()
        {
            ArmorBuff = false;
            EnemyChaseInstance = Instantiate(EnemyChaseBase);
            EnemyAttackInstance = Instantiate(EnemyAttackBase);


            //Contruct StateMachine
            StateMachine = new EnemyStateMachine();


            //Contruct States
            IdleState = new EnemyIdleState(this, StateMachine);
            ChaseState = new EnemyChaseState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            DedState = new EnemyDedState(this, StateMachine);
        }

        [HideInInspector] public bool ArmorBuff = false;
        [HideInInspector] public GameObject armorBuffVFX;
        public void AddArmorBuff(GameObject vfx)
        {
            if (!ArmorBuff)
            {
                armorBuffVFX = Instantiate(vfx, transform);
            }
            ArmorBuff = true;
            
        }

        public void RemoveArmorBuff()
        {
            if (ArmorBuff)
            {
                Destroy(armorBuffVFX);
            }
            ArmorBuff = false;

        }


        public virtual void Start()
        {
            
            if (IsOwner)
            {
                CurrentHealth.Value = MaxHealth;
                CurrentStaggerHealth.Value = MaxStaggerHealth;
            }
            //Set MaxHealth
            
            //Get References
            rb = GetComponent<Rigidbody>();
            if (!StaticEnemy)
            {
                navMesh = GetComponent<NavMeshAgent>();
                navMesh.avoidancePriority = Random.Range(0, 50);
                rb.isKinematic = true;
            }

            animator = GetComponent<Animator>();


            //Find Players in lobby
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }



            //Final Setup shit
            InitializeStateMachine();
            
            dissolve = GetComponent<DissolveController>();
            //
            if (!StaticEnemy)
            {
                SkinmeshRenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                if (SkinmeshRenderer == null)
                {
                    Debug.LogError("This Script Does not support non static enemy types without a skinnedMeshRenderer");
                }
                origColors = SkinmeshRenderer.materials;
                whites = SkinmeshRenderer.materials;
            }
            else
            {
                MR = gameObject.GetComponentInChildren<MeshRenderer>();
                origColors = MR.materials;
                whites = MR.materials;
            }
            
        }

        public virtual void InitializeStateMachine()
        {
            //Initialize StateMachine
            EnemyChaseInstance.Initialize(gameObject, this);
            EnemyAttackInstance.Initialize(gameObject, this);

            StateMachine.Initialize(ChaseState);
        }

        #region Health/Networking


        //So That can check for boss phase changes.
        public virtual void OnDamage()
        {
            
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            if (headshot) Damage *= 2;
            Damage = Damage * DamageReduction;
            CurrentHealth.Value -= (Damage);

            

            if (StaticEnemy)
            {
                if (CurrentHealth.Value<= 0)
                {
                    Die();
                }
            }



            else
            {
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

            OnDamage();
            aggression += 0.05f;
            HandleLocalVisualsClientRpc(Damage, headshot);
            

        }

        

        public void RegenStaggerHealth()
        {
            StaggerRegen = true;
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

        float floatattackDamage;

        [ServerRpc(RequireOwnership = false)]
        public void FloatAttackRecieveServerRpc(float Height, float Duration, float Damage)
        {
            if (StaticEnemy) return;

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
            if (StaticEnemy) return;
            PlayerSoundSource.Instance.PlaySound(SourceSoundManager.SoundType.LevitationHit,0.7f);
            transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.2f);
            DoDamageServerRpc(floatattackDamage);
            
            
            rb.isKinematic = false;
            animator.applyRootMotion = true;
            animator.Play("Hit", -1, 0);
            Invoke("EnableNavMeshServerRpc", 0.5f);
            Floating = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DisableNavMeshServerRpc()
        {
            if (StaticEnemy) return;
            
            else if (Floating) return;

            else if(StateMachine.currentState == DedState)
            {
                return;
            }
            else
            {

                navMesh.enabled = false;
                //animator.Play("Hit", -1, 0f);
                animator.applyRootMotion = false;
                Invoke("EnableNavMeshServerRpc", 1f);
            }

        }

        public void Die()
        {
            GameManager.instance.AwardXPServerRpc(xpOnKill);
            OnDeathTellSpawner();
            DisableHealthBarClientRpc();
            for (int i = 0; i < ChoasCoresOnDeath; i++)
            {
                Instantiate(ChaosCores, transform.position + 0.5f*new Vector3(Random.Range(-1f,1f),0.5f,Random.Range(-1f,1f)).normalized, Quaternion.identity);
            }
            if (!StaticEnemy)
            {
                Destroy(navMesh);
                CancelInvoke("EnableNavMeshServerRpc");

                if (Floating)
                {
                    transform.DOMove(-floatHeight * Vector3.up + transform.position, 0.1f);
                    animator.Play("FallingDeath", -1, 0);
                    CancelInvoke("EndFloatingEffectServerRpc");
                }
                else animator.Play("Die", -1, 0);

                dissolve.StartDissolveClientRpc();
                
                




                //Invoke("Delete", 4);
                foreach (var item in colliders)
                {
                    item.enabled = false;
                }
                Destroy(GetComponent<NetworkRigidbody>());
                Destroy(rb);

                Destroy(gameObject, 5);
            }
            else
            {
                Destroy(gameObject);
                
            }


        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNavMeshServerRpc()
        {
            if (StaticEnemy) return;
            if (rb == null) return;
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
            if (StaticEnemy)
            {
                MR.SetMaterials(whites.ToList());
            }
            else
            {
                SkinmeshRenderer.SetMaterials(whites.ToList());
            }
            


            Invoke("FlashEnd", flashTime);
        }

        void FlashEnd()
        {
            if (StaticEnemy)
            {
                MR.SetMaterials(origColors.ToList());
            }
            else
            {
                SkinmeshRenderer.SetMaterials(origColors.ToList());
            }
            
        }

        #endregion

        #endregion






        #endregion

        public void Attack()
        {

            RaycastHit hit;
           
            Physics.SphereCast(transform.position+0.5f*Vector3.up, 0.2f, transform.forward, out hit, MeleeRaycastRange, whatisPlayer);
            if (hit.collider != null)
            {

                hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(AttackDamage,transform.position);
            }


        }
        /*
        IEnumerator MoveAcrossNavMeshLink()
        {
            OffMeshLinkData data = navMesh.currentOffMeshLinkData;
            navMesh.updateRotation = false;

            Vector3 startPos = navMesh.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * navMesh.baseOffset;
            float duration = (endPos - startPos).magnitude / navMesh.velocity.magnitude;
            transform.position = endPos;
            navMesh.updateRotation = true;
            navMesh.CompleteOffMeshLink();
            MoveAcrossNavMeshesStarted = false;

        }
        */

        public void SpawnObj(GameObject spawnObj,Vector3 Pos)
        {

            SpawnedObj = spawnObj;
            SpawnObjServerRpc(Pos);
        }


        [ServerRpc(RequireOwnership = false)]
        public void SpawnObjServerRpc(Vector3 Pos)
        {
            
            Vector3 DirVec = -(transform.position - target.position).normalized;
            var projectile = Instantiate(SpawnedObj, Pos ,Quaternion.identity);
            projectile.GetComponent<NetworkObject>().Spawn();
            projectile.GetComponent<Projectile>().Direction = DirVec;
            
            Destroy(projectile,5);
        }


        public bool CheckLOS(out Vector3 opposingDirection)
        {
            RaycastHit hit;

            Physics.SphereCast(transform.position + 0.5f * Vector3.up, 0.6f, transform.forward, out hit, MeleeRaycastRange);
            opposingDirection = Vector3.zero;
            if (hit.collider==null) return true;
            if (hit.collider.gameObject.layer == gameObject.layer)
            {
                
                opposingDirection = (transform.position - hit.collider.transform.position).normalized;
                return false;
            }
            
            return true;
        }


        public void MoveEnemy(Vector3 targetPosition)
        {
            if (!navMesh.enabled) return;
            

            navMesh.SetDestination(targetPosition);
        }


        
        public virtual void AttackExit()
        {
            Attacking = false;
            animator.SetBool("Attacking", false);
        }


        public void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*RotateSpeed);
        }


        bool MoveAcrossNavMeshesStarted;

        public virtual void Update()
        {
            if(!IsOwner) return;
            if (!StaticEnemy)
            {
                Debug.DrawRay(transform.position + 0.5f * Vector3.up, transform.forward * 2f);
                StateMachine.currentState.FrameUpdate();
                if (navMesh == null) return;
                animator.SetFloat("SpeedX", Vector3.Dot(navMesh.velocity, transform.right));
                animator.SetFloat("SpeedY", Vector3.Dot(navMesh.velocity, transform.forward));

                if(StaggerRegen && CurrentStaggerHealth.Value < MaxStaggerHealth)
                {
                    CurrentStaggerHealth.Value += staggerRegenSpeed*Time.deltaTime;       
                        
                }
                /*
                if (navMesh.isOnOffMeshLink && !MoveAcrossNavMeshesStarted)
                {
                    StartCoRoutine(MoveAcrossNavMeshLink());
                    MoveAcrossNavMeshesStarted = true;
                }
                */
            }
            
        }

        private void FixedUpdate()
        {
            if(!IsOwner) return;
            StateMachine.currentState.PhysicsUpdate();
        }

        private void LateUpdate()
        {
            if(target != null)
            {
                LookAtTarget();
            }
        }

        public Transform DetectPlayer()
        {
            //print("Detecting Player...");
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
            PlayFootStepSound,
            SpawnProjectile
        }


        #endregion

    }
}
