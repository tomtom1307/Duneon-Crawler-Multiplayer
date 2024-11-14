using DG.Tweening;
using Project;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class WeaponSpawn : _Interactable
{
    
    public Item itemSO;
    public Transform ModelPos;
    public bool generateRarity;
    public bool includeVFX;

    ParticleSystem ps;

    //Table and rarity stuff

        public float[] table = 
        { 
        700, //Common 
        200, //Uncommon
        70,  //Rare
        29,  //Very Rare
        1,  //Legendary
        };

        public string[] rarity =
        {
            "Common",
            "Uncommon",
            "Rare",
            "Very Rare",
            "Legendary"
        };

        public Color[] Raritycolor =
        {
            Color.gray,
            Color.cyan,
            Color.blue,
            Color.magenta,
            Color.red
        };


    
    




    public List<GameObject> RarityEffects;

    private GameObject displayModel;
    [HideInInspector] public string Rarity;
    [HideInInspector] Color ItemColor;
    Inventory inventory;
    [HideInInspector] public Item item;
    Light pointLight;
    [HideInInspector] public float total;
    [HideInInspector] public float randomVal;
    [HideInInspector] public int index;


    //For Weapons 
    [SerializeField] WeaponInstance WI;


    public virtual void Start()
    {
        
        //Retrieve the Particle system and other reqs
        GetObjComponents();
        

        //Spawn the display model 
        displayModel = Instantiate(itemSO.model, ModelPos);
        displayModel.transform.DOMove(displayModel.transform.position + Vector3.up * 0.1f, 2).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        
        //Make non interactable
        displayModel.layer = 0;


        //Allow weapon to be interacted with
        isActive = true;

        //Do the same for subsequent meshes 
        List<MeshRenderer> MRS = displayModel.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var item in MRS)
        {
            item.gameObject.layer = 0;
        }
        
        //Set rotatin 
        displayModel.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        
        if(generateRarity)
        {
            //Calculate total value of table
            foreach (var it in table)
            {
                total += it;
            }
            //Generate random Value
            randomVal = Random.Range(0, total);
            index = 0;
            for (int i = 0; i < table.Length; i++)
            {
                if (randomVal <= table[i])
                {
                    //Establish the correct index 
                    index = i ; break;
                }
                else randomVal -= table[i];
            }

            //Set the item color
            ItemColor = Raritycolor[index];
            //Set the Rarity 
            Rarity = rarity[index];
        }

        //Retrieve inventory 
        inventory = Inventory.Singleton;

        //Initialize it as an inventory Item
        InitializeInventoryItem();
        if(includeVFX)
        {GameObject VFX = Instantiate(RarityEffects[index], transform);}
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
            case SlotTag.Weapon:
            {
                    
                    item.weaponData = itemSO.weaponData;
                    item.weaponData.model = item.model;
                    WI = new WeaponInstance(itemSO.weaponData, index);
                    item.weaponInstance = WI;
                    
                    break;
            }
            //For head armor
            case SlotTag.Head:
            {
                break;
            }
        }

        
        }


    private void GetObjComponents()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        pointLight = GetComponent<Light>();
    }




    //Interact function
    protected override void Interact()
    {
        if (!isActive) return;
        isActive = false;
        //Spawn into the inventory 
        inventory.SpawnInventoryItem(Raritycolor[index], item);

        //Destroy the visuals
        DOTween.Kill(displayModel.transform);
        base.Interact();
        Destroy(gameObject);
    }

    

}
