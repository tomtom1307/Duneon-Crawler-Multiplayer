using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public string description;
    public float coolDownTime;
    public float activeTime;

    public virtual void Activate()
    {

    }
}
