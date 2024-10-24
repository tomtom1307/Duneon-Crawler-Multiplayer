using Project;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public Sprite InventorySprite;
    public string description;
    public float coolDownTime;
    public float activeTime;
    public PlayerSoundSource.SoundType AssignedSound;




    public virtual void Activate(GameObject parent, out bool fail)
    {
        fail = false;
    }
}
