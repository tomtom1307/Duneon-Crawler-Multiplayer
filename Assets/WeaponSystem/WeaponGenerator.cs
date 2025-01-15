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

        [SerializeField] private List<WeaponComponent> ComponentsAlreadyOn = new List<WeaponComponent>();
        [SerializeField] private List<WeaponComponent> ComponentsAdded = new List<WeaponComponent>();

        [SerializeField] private List<Type> ComponentDependencies = new List<Type>();
        private GameObject ModelInstance;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            weaponStats = GetComponent<StatManager>();
        }



        public bool Generating;
        public void GenerateWeapon(WeaponInstance inst)
        {
            
            ComponentsAdded.Clear();

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
            Debug.Log("Dependancies!");
            foreach ( var  component in ComponentDependencies) {
                Debug.Log(component.Name);
            }

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
                StartCoroutine(weaponComponent.WaitForDataInitialization());
            }

            // Disable Unused Components
            var componentsToRemove = ComponentsAlreadyOn.Except(ComponentsAdded).ToList();

            foreach (var item in componentsToRemove)
            {
                item.InUse = false;
                Destroy(item);
            }

            

            Generating = false;
        }

        public void RemoveWeapon()
        {
            
            print("Removing Weapon");
            foreach( var component in ComponentsAlreadyOn)
            {
                Destroy(component);
            }
            
            Destroy(ModelInstance);

            ComponentsAlreadyOn.Clear();
            ComponentsAlreadyOn = GetComponents<WeaponComponent>().ToList();
        }

        public void SwapWeapon(WeaponInstance inst)
        {
            print("Swapping Weapon");
            //RemoveWeapon();
            GenerateWeapon(inst);

        }


        public void SpawnVisual(Transform parent, GameObject model) 
        { 
            ModelInstance = Instantiate(model, parent);
            ModelInstance.transform.localPosition = data.Position; 
            ModelInstance.transform.localEulerAngles = data.Rotation; 
            ModelInstance.transform.localScale = data.Scale; 
        }

    }
}