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

        if (Input.GetMouseButtonUp(1))
        {
            weapon.anim.SetBool("SecondaryRelease", true);
            Invoke("ResetCharge", 0.2f);
        }

        else if (Input.GetMouseButton(1))
        {
            weapon.Enter(2);
            ChargePercentage += (Time.deltaTime * ChargeSpeed);
            ChargePercentage = Mathf.Clamp(ChargePercentage, 0, 1);
            //StartCoroutine("Cooldown");
        }

        
        
        
        else if(Input.GetMouseButtonDown(0))
        {
            //StartCoroutine("Cooldown");
            weapon.Enter(1);
            
        }
        
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

