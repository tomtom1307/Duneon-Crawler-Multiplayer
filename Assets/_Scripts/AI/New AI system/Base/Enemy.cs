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
    public class Enemy : NetworkBehaviour, IDamageable, ITriggerCheckable
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

        [field: SerializeField] public GameObject HealthBarPrefab;
        [field: SerializeField] public float HealthBarHeight;
        Image HealthBar;
        Canvas HealthBarCanvas;

        [field: SerializeField] public float AttackDamage { get; set; } = 4f;
        
        [field: SerializeField] public int xpOnKill { get; set; }
        [field: SerializeField] public LayerMask whatisPlayer{ get; set; }
        public Rigidbody rb { get; set ; }
        

        public List<Collider> colliders;
        
        public Transform ProjectileSpawnPos;
        public Animator animator { get; set; }
        
        [HideInInspector]public List<Transform> playerlist;
        [HideInInspector] public Transform target;
        [HideInInspector] public float aggression = 0.5f;
        public EnemyStateMachine StateMachine { get; set; }
        public EnemyIdleState IdleState { get; set; }
        
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
        [HideInInspector] public SkinnedMeshRenderer SkinmeshRenderer;
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

        

        


        #endregion



        public virtual void Awake()
        {
            ArmorBuff = false;
            EnemyChaseInstance = Instantiate(EnemyChaseBase);
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
            //Set MaxHealth
            if (IsOwner)
            {
                CurrentHealth.Value = MaxHealth;
                CurrentStaggerHealth.Value = MaxStaggerHealth;
            }
            
            
            //Get References
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

            //Find Players in lobby
            List<Object> list = FindObjectsOfType(typeof(PlayerMovement)).ToList();
            foreach (var item in list)
            {
                playerlist.Add(item.GameObject().transform);
            }

            GameObject healthBarObj = Instantiate(HealthBarPrefab, transform);
            healthBarObj.transform.position = transform.position + HealthBarHeight * Vector3.up;
            HealthBar = healthBarObj.GetComponent<HealthBarEasing>().health;
            HealthBarCanvas = healthBarObj.GetComponent<Canvas>();


            //Final Setup shit
            InitializeStateMachine();
            
            dissolve = GetComponent<DissolveController>();
            //

          
            
        }

        public virtual void InitializeStateMachine()
        {
            //Initialize StateMachine
            
            EnemyAttackInstance.Initialize(gameObject, this);

            
        }

        #region Health/Networking


        //So That can check for boss phase changes.
        public virtual void OnDamage()
        {
            
            
        }


        [ServerRpc(RequireOwnership = false)]
        public virtual void DoDamageServerRpc(float Damage, bool headshot = false)
        {
            if (headshot) Damage *= 2;
            Damage = Damage * DamageReduction;
            CurrentHealth.Value -= (Damage);

            OnDamage();
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



        public virtual void Die()
        {
            GameManager.instance.AwardXPServerRpc(xpOnKill);
            OnDeathTellSpawner();
            DisableHealthBarClientRpc();
            for (int i = 0; i < ChoasCoresOnDeath; i++)
            {
                Instantiate(ChaosCores, transform.position + 0.5f*new Vector3(Random.Range(-1f,1f),0.5f,Random.Range(-1f,1f)).normalized, Quaternion.identity);
            }
            
            
                


        }

        
        [ClientRpc]
        void DisableHealthBarClientRpc()
        {
            HealthBarCanvas.enabled = false;
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

        public void Attack()
        {

            RaycastHit hit;
           
            Physics.SphereCast(transform.position+0.5f*Vector3.up, 0.2f, transform.forward, out hit, MeleeRaycastRange, whatisPlayer);
            if (hit.collider != null)
            {

                hit.collider.gameObject.GetComponent<PlayerStats>().TakeDamage(AttackDamage,transform.position);
            }
        }

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


        public virtual void Update()
        {
            if(!IsOwner) return;
            
            
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
