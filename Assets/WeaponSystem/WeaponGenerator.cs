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
        Animator anim;

        private List<WeaponComponent> ComponentsAlreadyOn = new List<WeaponComponent>();
        private List<WeaponComponent> ComponentsAdded = new List<WeaponComponent>();

        private List<Type> ComponentDependencies = new List<Type>();
        private GameObject ModelInstance;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            weaponStats = GetComponent<StatManager>();
        }



        public bool Generating;
        public void GenerateWeapon(WeaponInstance inst)
        {
            data = inst.weaponData;
            weapon.SetData(data);
            Generating = true;

            // Set weapon visuals and stats
            weapon.visualAttacks.Attack1PS = data.Attack1VFX;
            weapon.visualAttacks.Attack2PS = data.Attack2VFX;
            weaponStats.ChangeStats(inst);

            print("Generating Weapon");

            // Destroy old model if exists
            if (ModelInstance != null)
            {
                Destroy(ModelInstance);
            }
            SpawnVisual(weaponPos, data.model);
            anim.runtimeAnimatorController = data.AnimController;

            // Get all dependencies from data
            ComponentDependencies = data.GetAllDependencies();
            print(ComponentDependencies);

            // Enable Required Components
            foreach (var dependency in ComponentDependencies)
            {
                var weaponComponent = ComponentsAlreadyOn.FirstOrDefault(component => component.GetType() == dependency);

                if (weaponComponent == null)
                {
                    weaponComponent = gameObject.AddComponent(dependency) as WeaponComponent;
                    ComponentsAlreadyOn.Add(weaponComponent);
                }

                if (!ComponentsAdded.Contains(weaponComponent))
                {
                    ComponentsAdded.Add(weaponComponent);
                }

                weaponComponent.InUse = true;
            }

            // Disable Unused Components
            var componentsToRemove = ComponentsAlreadyOn.Except(ComponentsAdded).ToList();

            foreach (var item in componentsToRemove)
            {
                item.InUse = false;
                ComponentsAdded.Remove(item); // Remove from active components
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
            RemoveWeapon();
            GenerateWeapon(inst);

        }


        public void SpawnVisual(Transform parent, GameObject model) 
        { 
            ModelInstance = Instantiate(model, parent);
        }

    }
}