using Project;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PickUpItem : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    public WeaponDataSO WeaponData;
    public TextMeshProUGUI prompt;
    public KeyCode PickUpKey = KeyCode.F;
    public Color ItemColor;
    ParticleSystem ps;
    public Transform ModelPos;
    public string Rarity;




    public float[] table = 
    { 700, //Common 
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
    Item item;
    public bool PickupB = true;
    Light pointLight;
    public float total;
    public float randomVal;
    int index;
    private void Start()
    {
        
        prompt.enabled = false;
        GetObjComponents();
        InitializeInventoryItem();
        GameObject displayModel = Instantiate(WeaponData.Model,ModelPos);
        displayModel.transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));

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
                index = i ; break;
            }
            else randomVal -= table[i];
        } 
            
        ItemColor = Raritycolor[index];
        pointLight.color = Raritycolor[index];
        ps.startColor = Raritycolor[index];

        Rarity = rarity[index];


    }

    private void InitializeInventoryItem()
    {
        item = new Item();
        item.name = WeaponData.name;
        item.sprite = WeaponData.InventorySprite;
        item.itemTag = WeaponData.itemTag;
        item.prefab = WeaponData.Model;
    }


    private void GetObjComponents()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        pointLight = GetComponent<Light>();
    }

    private void OnTriggerStay(Collider other)
    {
        prompt.enabled = true;
        if (Input.GetKeyDown(PickUpKey)&&PickupB)
        {

            PickupB = false;
            Pickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        prompt.enabled = false;
    }


    public void Pickup()
    {

        inventory.SpawnInventoryItem(Raritycolor[index],item);

        Destroy(gameObject);
        
        
    }

}
