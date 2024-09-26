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
            if (ModelInstance != null)
            {
                Destroy(ModelInstance);
            }
            SpawnVisual(weaponPos,data.model);

            ComponentDependencies.Clear();
            ComponentsAlreadyOn.Clear();
            ComponentDependencies.Clear();

            ComponentDependencies = data.GetAllDependencies();

            foreach (var dependancy in ComponentDependencies)
            {
                if(ComponentsAdded.FirstOrDefault(component => component.GetType() == dependancy))
                {
                    continue;
                }

                var weaponComponent = ComponentsAlreadyOn.FirstOrDefault(component => component.GetType() == dependancy);

                if(weaponComponent == null)
                {
                    weaponComponent = gameObject.AddComponent(dependancy) as WeaponComponent;
                }

                ComponentsAdded.Add(weaponComponent);

            }

            var componentsToRemove = ComponentsAlreadyOn.Except(ComponentsAdded);

            foreach (var item in componentsToRemove)
            {
                Destroy(item);
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

        public void SwapWeapon(WeaponInstance inst)
        {
            print("Swapping Weapon");
            GenerateWeapon(inst);

        }


        public void SpawnVisual(Transform parent, GameObject model) 
        { 
            ModelInstance = Instantiate(model, parent);
        }

    }
}