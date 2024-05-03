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
            StartCoroutine("Cooldown");
        }
        else if(Input.GetMouseButtonUp(1))
        {
            weapon.anim.SetBool("SecondaryRelease", true);
        }
        if (Attacking) return;
        else if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine("Cooldown");
            Enter();
        }
        
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

