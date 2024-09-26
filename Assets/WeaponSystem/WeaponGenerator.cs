using Project.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Assets.WeaponSystem
{
    public class WeaponGenerator : MonoBehaviour
    {
        [SerializeField] private WeaponHolder weapon;
        [SerializeField] private WeaponDataSO data;
        [SerializeField] private Transform weaponPos;
        [SerializeField] private StatManager weaponStats;

        private List<WeaponComponent> ComponentsAlreadyOn = new List<WeaponComponent>();
        private List<WeaponComponent> ComponentsAdded = new List<WeaponComponent>();

        private List<Type> ComponentDependencies = new List<Type>();
        private GameObject ModelInstance;

        private void Start()
        {
            weaponStats = GetComponent<StatManager>();
        }

        

        public bool Generating;
        public void GenerateWeapon(WeaponInstance inst)
        {
            data = inst.weaponData;
            Generating = true;
            weaponStats.ChangeStats(inst);
            print("generating Weapon");
            weapon.SetData(data);
            SpawnVisual(weaponPos,data.model);
            

            ComponentDependencies.Clear();
            ComponentsAlreadyOn.Clear();
            
            ComponentDependencies = data.GetAllDependencies();

            foreach (var dependancy in ComponentDependencies)
            {
                var component = gameObject.AddComponent(dependancy) as WeaponComponent;
            }
            Generating = false;
        }
        public void RemoveWeapon(WeaponInstance inst = null)
        {
            
            print("Removing Weapon");
            ComponentsAlreadyOn = GetComponents<WeaponComponent>().ToList();
            foreach (var item in ComponentsAlreadyOn)
            {
                Destroy(item);
            }
            Destroy(ModelInstance);
            if (inst != null)
            {
                GenerateWeapon(inst);
                
            }
        }

        public void SwapWeapon(WeaponInstance inst)
        {
            print("Swapping Weapon");
            RemoveWeapon();
            GenerateWeapon(inst);

        }


        public void SpawnVisual(Transform parent, GameObject model) 
        { 
            ModelInstance = Instantiate(model, parent);
        }

    }
}