using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : ScriptableObject
{
    public new string name;
    public string description;
    public string type;
    public float PhysicalDamage;
    public float MagicDamage;
    
    

    public virtual void PrimaryAttack()
    {

    }


    public virtual void SecondaryAttack() 
    { 
    
    }
}
