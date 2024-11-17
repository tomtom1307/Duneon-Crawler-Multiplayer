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
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Enemy_AnimatorEventHandler))]
    public class Enemy : NetworkBehaviour, ITriggerCheckable
    {
        public float SpawnFXSize = 1;
        #region Definitions

        #region Hidden Stats

        [HideInInspector] public float DamageReduction = 1;
        public NetworkVariable<float> CurrentStaggerHealth { get; set; } = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> CurrentHealth { get; set; } = new NetworkVariable<float>(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Owner);
        #endregion

        #region Old Attack System
        [field: SerializeField] public float AttackDistance { get; set; } = 4f;
        [field: SerializeField] public float MeleeRaycastRange { get; set; } = 0.7f;

        #endregion

        #region HealthBarStuff
        Image HealthBar;
        Canvas HealthBarCanvas;
        #endregion

        public Rigidbody rb { get; set; }
        public List<Collider> colliders { get; set; }
        private List<Enemy_Attack_ColliderDetector> colliderDetector;
        public Animator animator { get; set; }
        [HideInInspector]public List<Transform> playerlist;
        [HideInInspector] public Transform target;
        [HideInInspector] public float aggression = 0.5f;

        #region TriggerBools

        public bool PlayerIsTooClose { get; set; }
        public bool IsWithinStrikingDistance { get; set; }

        #endregion

        #region VFX_var
        [HideInInspector] public Material[] origColors { get; set; }
        [HideInInspector] public Material[] whites { get; set; }
        public float flashTime { get; set; } = 1f;
        public DissolveController dissolve { get; set; }
        #endregion

        public GameObject SpawnedObj { get; set; }
        public GameObject DesignatedRoom { get; set; }

        //VFX Stuff
        [HideInInspector] public SkinnedMeshRenderer SkinmeshRenderer;
        
        public bool Attacking;

        

        [Header("General Assginments")]
        public GameObject ChaosCores;
        public GameObject damageText;
        [field: SerializeField] public Color HeadshotColor = Color.HSVToRGB(0, 7.6f, 1.00f);
        [field: SerializeField] public Color NormalColor = Color.HSVToRGB(0, 0, 1.00f);
        [field: SerializeField] public LayerMask whatisPlayer;
        [field: SerializeField] public GameObject HealthBarPrefab;
        [field: SerializeField] public float HealthBarHeight;



        [Header("Special Assignments")]
        public Transform ProjectileSpawnPos;



        [Header("Enemy Stats")]
        public float MaxHealth = 100f;
        public float MaxStaggerHealth;
        public float maxDetectDist = 100f;
        public float RotateSpeed = 100f;
        
        [field: SerializeField] public float AttackDamage { get; set; } = 4f;
        [field: SerializeField] public int xpOnKill { get; set; }
        public int ChoasCoresOnDeath;


        
        [Header("Enemy States")]
        [SerializeField] private EnemyAttackSOBase EnemyAttackBase;
        
        public Enemy_Attack currentAttack {  get; set; }
        public PlayerStats currentPlayer { get; set; }

        #region StateMachine Variables

        public EnemyStateMachine StateMachine { get; set; }

        public EnemyDedState DedState { get; set; }
        public EnemyIdleState IdleState { get; set; }
        public EnemyAttackState AttackState { get; set; }


        public EnemyChaseSOBase EnemyChaseInstance { get; set; }
        public EnemyAttackSOBase EnemyAttackInstance { get; set; }

        #endregion

        [HideInInspector] public EnemySpawner Spawner;
        #endregion

        public virtual void Awake()
        {
            //Disable armor buff
            ArmorBuff = false;

            //Instantiate StateMachine States
            EnemyAttackInstance = Instantiate(EnemyAttackBase);


            //Contruct StateMachine
            StateMachine = new EnemyStateMachine();


            //Contruct States
            IdleState = new EnemyIdleState(this, StateMachine);
            AttackState = new EnemyAttackState(this, StateMachine);
            DedState = new EnemyDedState(this, StateMachine);
        }

        [HideInInspector] public bool ArmorBuff = false;
        [HideInInspector] public GameObject armorBuffVFX;

        #region ArmorBuff
        //Adds a buff
        public void AddArmorBuff(GameObject vfx)
        {
            if (!ArmorBuff)
            {
                armorBuffVFX = Instantiate(vfx, transform);
            }
            ArmorBuff = true;
            
        }


        //Removes Buff
        public void RemoveArmorBuff()
        {
            if (ArmorBuff)
            {
                Destroy(armorBuffVFX);
            }
            ArmorBuff = false;

        }
        #endregion


        #region StartFunctions
        public void InitializeHealthValues()
        {
            if (IsOwner)
            {
                CurrentHealth.Value = MaxHealth;
                CurrentStaggerHealth.Value = MaxStaggerHealth;
            }
        }

        public void GetReferences()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            dissolve = GetComponent<DissolveController>();

            GetColliders();
        }

        public void FindPlayers()
        {
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }
        }

        public void SetUpHealthBar()
        {
            GameObject healthBarObj = Instantiate(HealthBarPrefab, transform);
            healthBarObj.transform.position = transform.position + HealthBarHeight * Vector3.up;
            HealthBar = healthBarObj.GetComponent<HealthBarEasing>().health;
            HealthBarCanvas = healthBarObj.GetComponent<Canvas>();
        }

        public virtual void InitializeStateMachine()
        {
            //Initialize StateMachine

            EnemyAttackInstance.Initialize(gameObject, this);


        }


        public virtual void GetColliders()
        {
            colliders = GetComponentsInChildren<Collider>().ToList();
            colliders.Add(GetComponent<Collider>());

            colliderDetector = GetComponentsInChildren<Enemy_Attack_ColliderDetector>().ToList();

            foreach (var item in colliderDetector)
            {
                item.playerDetected += DamagePlayer;
            }

        }
        #endregion

        public virtual void Start()
        {

            InitializeHealthValues();

            GetReferences();

            FindPlayers();

            SetUpHealthBar();

            InitializeStateMachine();

            target = DetectPlayer();
        }

        

        #region Receiving Damage and KnockBack


        //So That can check for boss phase changes.
        public virtual void OnDamage()
        {
            
            
        }


        [ServerRpc(RequireOwnership = false)]
        public virtual void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            //Check if headshot and double damage if true
            if (headshot) Damage *= 2;

            //Apply damage reduction
            Damage = Damage * DamageReduction;

            //Apply Damage
            CurrentHealth.Value -= (Damage);

            //Call for additional checks ie Boss Phase Change
            OnDamage();

            //Increment aggression if we want to keep this as a factor 
            aggression += 0.05f;

            //Call Damage Flash and Numbers
            HandleLocalVisualsClientRpc(Damage, headshot);
            

        }
        

        [ServerRpc(RequireOwnership = false)]
        public void KnockBackServerRpc(Vector3 playerPos, float KnockBack = 5)
        {
            
            rb.isKinematic = false;

            //Find the knockback direction and apply it
            Vector3 dir = transform.position - playerPos;
            rb.AddForce(dir.normalized * KnockBack, ForceMode.Force);
        }



        public virtual void Die()
        {
            //Award XP to all players
            GameManager.instance.AwardXPServerRpc(xpOnKill);

            //tell the spawner this enemy is dead
            OnDeathTellSpawner();

            DisableHealthBarClientRpc();

            SpawnChaosCores();
        }

        


        public void SpawnChaosCores()
        {
            for (int i = 0; i < ChoasCoresOnDeath; i++)
            {
                Instantiate(ChaosCores, transform.position + 0.5f * new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f)).normalized, Quaternion.identity);
            }
        }
        
        public virtual void OnDeathTellSpawner()
        {
            if (Spawner != null)
            {
                Spawner.EnemiesLeft -= 1;
            }
        }


        #region DamageVFX
        [ClientRpc]
        void DisableHealthBarClientRpc()
        {
            HealthBarCanvas.enabled = false;
        }


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


            indicator.SetDamageText(Mathf.RoundToInt(dam), color);
        }

        #region Damage Flash

        void DamageFlash()
        {
            FlashStart();
        }



        public virtual void FlashStart()
        {

            Invoke("FlashEnd", flashTime);
        }

        public virtual void FlashEnd()
        {
            
            
        }

        #endregion

        #endregion






        #endregion



        #region Attacking

        public void TriggerAttack(Enemy_Attack EA)
        {
            Attacking = true;
            currentAttack = EA;
            currentAttack.EnterLogic(this);
            animator.SetInteger("AttackInt", EA.AnimationIntValue);
            animator.SetBool("Attacking", true);
            
            
        }


        public virtual void Attack()
        {
            SphereCastAttack();
        }

        public void SphereCastAttack()
        {
            RaycastHit hit;
           
            Physics.SphereCast(transform.position+0.5f*Vector3.up, 0.2f, transform.forward, out hit, MeleeRaycastRange, whatisPlayer);
            if (hit.collider != null)
            {

                hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(AttackDamage,transform.position);
            }
        }

        public void DamagePlayer(PlayerStats ps)
        {
            //Cast Attack as an AOE attack if this works then Do AOE logic
            Enemy_Attack_AOE_Base AOEAttack = currentAttack as Enemy_Attack_AOE_Base;

            if (AOEAttack != null)
            {
                //Distance to Player
                float Distance = Vector3.Distance(transform.position, ps.transform.position);
                //Apply Damage Distance Scaling
                float DamageVal = currentAttack.Damage * AOEDamageFalloffFunction(Distance, AOEAttack.AttackRadius);

                //Apply new Damage Value
                ps.TakeDamage(DamageVal, transform.position);
            }

            //Otherwise
            else
            {
                //Damage the player
                ps.TakeDamage(currentAttack.Damage, transform.position);
            }
            PlayerHitLogic(ps);

        }

        public void SpawnProjectile(GameObject spawnObj,Vector3 Pos, float Speed, float Damage, bool Gravity = true)
        {
            //Set Reference to object since gameobjects can't be passed as variables for Rpc's
            SpawnedObj = spawnObj;

            //Call Rpc
            SpawnObjServerRpc(Pos, Speed, Damage, Gravity);
        }


        [ServerRpc(RequireOwnership = false)]
        public void SpawnObjServerRpc(Vector3 Pos, float Speed, float Damage, bool Gravity)
        {
            Vector3 DirVec = -(ProjectileSpawnPos.position - currentPlayer.transform.position).normalized;
            var projectile = Instantiate(SpawnedObj, Pos ,Quaternion.identity);
            projectile.GetComponent<NetworkObject>().Spawn();


            Projectile proj = projectile.GetComponent<Projectile>();
            proj.Direction = DirVec;
            proj.Speed = Speed;
            proj.damage = Damage;
            proj.GetComponent<Rigidbody>().useGravity = Gravity;
            
            Destroy(projectile,5);
        }


        public void SpawnFX(GameObject Go)
        {
            Instantiate(Go, transform.position, Quaternion.identity);
        }

        
        public bool LOS(Vector3 TargetPos)
        {
            if(Physics.Raycast(transform.position, TargetPos - transform.position, 100, whatisPlayer))
            {
                return true;
            }
            else
            {
                return false;
            }
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


        public virtual void AttackExit()
        {
            if (currentAttack != null)
            {
                currentAttack.ExitLogic(this);
                currentAttack = null;
            }
            
            
            animator.SetBool("Attacking", false);
            animator.SetInteger("AttackInt", 0);
            Attacking = false;
        }


        public void AttackDetect()
        {
            currentAttack.Detect(this);
        }



        public void StopDetect()
        {
            currentAttack.StopDetecting(this);
        }


        //Function for adding forces and such to each player
        public void PlayerHitLogic(PlayerStats ps)
        {
            currentAttack.playerHitLogic(ps ,this);
            currentPlayer = ps;
        }


        public void DoAttackLogic()
        {
            currentAttack.Attack(currentPlayer ,this);
        }

        #region Detection Helper Functions

        public void EnableMeleeCollider()
        {
            foreach (var detector in colliderDetector)
            {
                detector.TriggerCollider(true);
            }
        }

        public void DisableMeleeCollider()
        {
            foreach (var detector in colliderDetector)
            {
                detector.TriggerCollider(false);
            }
        }

        public void DoOverlapSphere(float radius)
        {

            // Create a list if playerColliders
            List<Collider> players = new List<Collider>();

            //Check players within a sphere of a certain radius
            players = Physics.OverlapSphere(transform.position, radius, whatisPlayer).ToList();

            //Check collider has Stats to then apply damage
            foreach (var collider in players) 
            {
                PlayerStats PS;
                if(collider.TryGetComponent<PlayerStats>(out PS))
                {
                    DamagePlayer(PS);
                }
            }
        }

        public float AOEDamageFalloffFunction(float playerDistance, float r)
        {
            return (-1 / 1.7f)* Mathf.Pow(playerDistance/ r, 2) + 1; 
        }

        public void EnableNavMesh(bool x)
        {
            //Cast this script as a NavmeshEnemy script if this works then trigger respective RPC logic
            NavMeshEnemy NME = this as NavMeshEnemy;

            if (NME != null)
            {
                if (x)
                {
                    NME.EnableNavMeshServerRpc();
                }
                else if (!x)
                {
                    NME.DisableNavMeshServerRpc(true);
                }
            }

            //Otherwise
            else
            {
                return;
            }

        }


        #endregion



        #endregion

        public virtual void LookAtTarget()
        {
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime*RotateSpeed);
        }


        public virtual void Update()
        {
            //if(!IsOwner) return;
            StateMachine.currentState.FrameUpdate();
            
        }

        private void FixedUpdate()
        {
            if(!IsOwner) return;
            //print(StateMachine.currentState);
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
            
            float MinDist = maxDetectDist;
            Transform target = null;
            foreach (var item in playerlist)
            {
                if (Vector3.Distance(transform.position, item.position) < MinDist)
                {
                    MinDist = Vector3.Distance(transform.position, item.position);
                    target = item;
                    currentPlayer = item.GetComponent<PlayerStats>();
                }
            }


            return target;
        }



        #region State Change Triggers

        public void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            //if (!IsOwner) return;
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
            SpawnProjectile,
            AttackDetection,
            SpawnVFX
        }


        #endregion

    }
}
