using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Project
{
    public enum DamageType
    {
        Magic,
        Physical
    }

    [Serializable]
    [CreateAssetMenu(menuName = "newWeaponData")]
    public class WeaponDataSO : ScriptableObject
    {

        [field: SerializeField] public float ChargeUpSpeed;

        [field: SerializeField] public WeaponInputsSO Inputs;
        
        [field: SerializeField] public AnimatorController AnimController{ get; private set; }
        [field: SerializeField] public float Attack1Cooldown { get; private set; }
        [field: SerializeField] public float Attack2Cooldown { get; private set; }
        [field: SerializeField] public float Attack1ManaUse { get; private set; }
        [field: SerializeField] public float Attack2ManaUse { get; private set; }
        [field: SerializeField] public DamageType Attack1Type { get; private set; }
        [field: SerializeField] public DamageType Attack2Type { get; private set; }
        [field: SerializeField] public GameObject Attack1VFX { get; private set; }
        [field: SerializeField] public GameObject Attack2VFX { get; private set; }

        [field: SerializeField] public float AttackSpeed { get; private set; }

        [field: SerializeReference] public List<ComponentData> componentDatas { get; private set; }

        [field: SerializeField] public int NumberOfAttacks { get; private set; }
        [field: SerializeField] public int NumberOfAttacks1 { get; private set; }
        [field: SerializeReference] public List<ComponentData> componentDatas1 { get; private set; }

        [field: SerializeField] public int NumberOfAttacks2 { get; private set; }
        [field: SerializeReference] public List<ComponentData> componentDatas2 { get; private set; }

        [HideInInspector] public GameObject model;

        public T GetData<T>(int data = 0)
        {
            

            if(data == 1)
            {
                return componentDatas1.OfType<T>().FirstOrDefault();
            }


            if (data == 2)
            {
                return componentDatas2.OfType<T>().FirstOrDefault();
            }

            else
            {
                return componentDatas.OfType<T>().FirstOrDefault();
            }

        }




        public List<Type> GetAllDependencies()
        {
            // Combine dependencies from componentDatas1 (Attack1) and componentDatas2 (Attack2)
            var allDependencies = componentDatas1
                .Select(component => component.ComponentDependancy)
                .Union(componentDatas2.Select(component => component.ComponentDependancy)) // Ensures no duplicates
                .ToList();

            // Return the combined list without duplicates
            return allDependencies;
        }

        public void AddData(ComponentData data)
        {
            if (componentDatas.FirstOrDefault(t => t.GetType() == data.GetType()) != null) return;
            
            componentDatas.Add(data);
        }


        // Add data to Attack1
        public void AddDataToAttack1(ComponentData data)
        {
            componentDatas1.Add(data);
        }

        // Add data to Attack2
        public void AddDataToAttack2(ComponentData data)
        {
            componentDatas2.Add(data);
        }







    }
}
