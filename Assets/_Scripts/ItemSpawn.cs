using DG.Tweening;
using Project;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class ItemSpawn : _Interactable
{
    
    public Item itemSO;

    private static Dictionary<Rarity,Color> RarityColor;
    ParticleSystem ps;
    public float floatTime;
    public float floatHeight;

    private GameObject displayModel;

   
    Inventory inventory;
    [HideInInspector] public Item item;
    Light pointLight;
    Sequence itemSeq;


    public virtual void Start()
    {
        //Initialize Rarity color dictionary
        RarityColor = new Dictionary<Rarity, Color>()
        {
            {Rarity.Common, Color.gray},
            {Rarity.Uncommon, Color.cyan},
            {Rarity.Rare, Color.blue},
            {Rarity.Very_Rare, Color.magenta},
            {Rarity.Legendary, Color.red}
        };

        //Retrieve the Particle system and other reqs
        GetObjComponents();
        

        //Spawn the display model 
        displayModel = Instantiate(itemSO.model, transform);


        
        //Make non interactable
        displayModel.layer = 0;

        //Do the same for subsequent meshes 
        List<MeshRenderer> MRS = displayModel.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var item in MRS)
        {
            item.gameObject.layer = 0;
        }
        
    
        //Retrieve inventory 
        inventory = Inventory.Singleton;

        //Initialize it as an inventory Item
        InitializeInventoryItem();
    }

    
    //Setting all the values for the instanced ItemSO
    private void InitializeInventoryItem()
    {
        item = ScriptableObject.CreateInstance<Item>();
        item.name = itemSO.name;
        item.inventorySprite = itemSO.inventorySprite;
        item.itemTag = itemSO.itemTag;
        item.model = itemSO.model;
        item.itemID = itemSO.itemID;


        //Check itemTag for special initialization conditions
        switch(item.itemTag)
        {
            //For Weapons
            case SlotTag.Objective:
            {
                    break;
            }
            //For head armor
            case SlotTag.Head:
            {
                break;
            }
        }

        
        }

    public void FloatUp()
    {
        Debug.Log("Float sequence triggered.");
        //Initialize Sequence
        itemSeq = DOTween.Sequence();
        itemSeq.Append(transform.DOMove(transform.position + Vector3.up*floatHeight,floatTime).SetEase(Ease.InOutSine));
        itemSeq.Insert(floatTime*0.8f,transform.DORotate(new Vector3(0f,180f,0f),1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental));
        itemSeq.Insert(floatTime, transform.DOMove(transform.position + Vector3.up * (floatHeight- 0.1f), 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo));
    }

    private void GetObjComponents()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        pointLight = GetComponent<Light>();
    }




    //Interact function
    protected override void Interact()
    {
        //Spawn into the inventory 
        inventory.SpawnInventoryItem(Color.white, item);

        //Destroy the visuals 
        
        base.Interact();
        itemSeq.Kill();
        Destroy(gameObject);
    }

    

}
