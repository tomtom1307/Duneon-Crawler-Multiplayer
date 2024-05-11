using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilitytable : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        anim.Play("BookOpen");
    }

    private void OnTriggerStay(Collider other)
    {
        anim.SetBool("In", true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("In", false);
    }
}
