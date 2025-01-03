using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Weapons;
using Unity.Netcode;
using Project;


public class PlayerAttack : NetworkBehaviour
{

    private WeaponHolder weapon;
    [SerializeField] public bool Attacking;
    [SerializeField] public float AttackCooldown;
    [SerializeField] private Camera cam;
    public int mouseButton;
    public float ChargePercentage;
    public float ChargeSpeed = 3f;
    public bool fullyCharged;
    private void Awake()
    {
        cam = Camera.main;
        weapon = cam.GetComponentInChildren<WeaponHolder>();
        weapon.OnExit += ExitHandler;
        Attacking = false;
        
    }


    private void Update()
    {


        if (weapon.State == WeaponHolder.AttackState.coolDown) return;

        weapon.Data.Inputs.InputLogic(weapon,this);
        
    }

    public void ResetCharge()
    {
        ChargePercentage = 0;
        weapon.anim.SetBool("SecondaryRelease", false);
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

