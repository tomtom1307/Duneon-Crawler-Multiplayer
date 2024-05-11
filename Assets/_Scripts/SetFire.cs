using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFire : MonoBehaviour
{
    public GameObject PS;
    public float FireTime;
    GameObject FireParticles;

    public void StartFire()
    {
        FireParticles = Instantiate(PS,this.transform, false);
        //StartCoroutine(SelfDestruct(FireTime));
        Destroy(FireParticles,FireTime);
    }

    IEnumerator SelfDestruct(float timer)
    {
        yield return new WaitForSeconds(5f);
        DestroyImmediate(FireParticles);
    }

}
