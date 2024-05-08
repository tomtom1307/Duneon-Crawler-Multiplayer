using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Weapons;
using Unity.Netcode;


public class PlayerAttack : NetworkBehaviour
{

    private WeaponHolder weapon;
    [SerializeField] public bool Attacking;
    [SerializeField] public float AttackCooldown;
    [SerializeField] private Camera cam;
    public int mouseButton;
    public float ChargePercentage;
    public float ChargeSpeed = 3f;

    private void Awake()
    {
        cam = Camera.main;
        weapon = cam.GetComponentInChildren<WeaponHolder>();
        weapon.OnExit += ExitHandler;
        Attacking = false;
        
    }

    void Enter()
    {
        
        weapon.Enter();
        Attacking = true;
    }

    private void Update()
    {
        
        
        
        if(Input.GetMouseButton(1))
        {
            weapon.EnterSecondaryAttack();
            ChargePercentage += (Time.deltaTime * ChargeSpeed);
            ChargePercentage = Mathf.Clamp(ChargePercentage, 0, 1);
            StartCoroutine("Cooldown");
        }
        else if(Input.GetMouseButtonUp(1))
        {
            Invoke("ResetCharge", 0.3f);
            weapon.anim.SetBool("SecondaryRelease", true);
        }
        if (Attacking) return;
        else if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine("Cooldown");
            Enter();
        }
        
    }

    public void ResetCharge()
    {
        ChargePercentage = 0;

    }



    IEnumerator Cooldown()
    {
        Attacking = true;
        yield return new WaitForSeconds(AttackCooldown);
        Attacking = false;
    }


    private void ExitHandler()
    {
        Attacking = false;

        
        
    }

    private void OnEnable()
    {
        Attacking = false;
    }

    private void OnDisable()
    {
        Attacking = false;
    }



}

