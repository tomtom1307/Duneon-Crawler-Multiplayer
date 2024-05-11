using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Training_Dummy : NetworkBehaviour
{
    


    [Header("ResetDummy")]
    public Vector3 startPosition;
    public Quaternion startRotation;
    public float ResetTime;
    public float ResetDistance;
    
    public bool reset = true;
    float Health;
    float MaxHealth;
    Image HealthBar;
    Animator anim;
    public GameObject PoofParticles;

    TakeDamage TD;
    Camera cam;
    Rigidbody rb;

    void Start()
    {
        cam = Camera.main;
        TD = GetComponent<TakeDamage>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        MaxHealth = TD._health.Value;
        anim = GetComponent<Animator>();
  
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        

        if(Vector3.Distance(transform.position,startPosition) > ResetDistance && rb.velocity.magnitude <1f && reset)
        {

            StartCoroutine(ResetDummy());
            reset = false;
        }
    }
    
    

    
    public void Poof()
    {
        GameObject pp = Instantiate(PoofParticles,this.gameObject.transform);
        Destroy(pp,4);
    }


    IEnumerator ResetDummy()
    {
        
        yield return new WaitForSeconds(ResetTime);
        anim.Play("Reset");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (NetworkManager.Singleton.LocalClientId == 0)
        {
            TD._health.Value = MaxHealth;

        }

        

    }


    public void MoveDummy()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        anim.Play("IdleTrainingDummy");
        TD.HealthBar.fillAmount = TD._health.Value / TD.MaxHealth;
        reset = true;
    }
}
