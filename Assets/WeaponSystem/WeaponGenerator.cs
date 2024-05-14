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

        private List<WeaponComponent> ComponentsAlreadyOn = new List<WeaponComponent>();
        private List<WeaponComponent> ComponentsAdded = new List<WeaponComponent>();

        private List<Type> ComponentDependencies = new List<Type>();
        private GameObject ModelInstance;

        private void Start()
        {
            //GenerateWeapon(data);
        }

        [ContextMenu("TextGenerate")]
        private void TestGeneration()
        {
            GenerateWeapon(data);
        }

        public bool Generating;
        public void GenerateWeapon(WeaponDataSO data)
        {
            Generating = true;
            print("generating Weapon");
            weapon.SetData(data);
            SpawnVisual(weaponPos,data.Model);
            
            ComponentDependencies.Clear();
            ComponentsAlreadyOn.Clear();
            
            ComponentDependencies = data.GetAllDependencies();

            foreach (var dependancy in ComponentDependencies)
            {
                var component = gameObject.AddComponent(dependancy) as WeaponComponent;
            }
            Generating = false;
        }
        public void RemoveWeapon()
        {
            print("Removing Weapon");
            ComponentsAlreadyOn = GetComponents<WeaponComponent>().ToList();
            foreach (var item in ComponentsAlreadyOn)
            {
                Destroy(item);
            }
            Destroy(ModelInstance);
        }


        public void SpawnVisual(Transform parent, GameObject model) 
        { 
            ModelInstance = Instantiate(model, parent);
        }

    }
}