using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssFire : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SetFire SF = other.gameObject.GetComponentInChildren<SetFire>();
        SF?.StartFire();
    }
}
