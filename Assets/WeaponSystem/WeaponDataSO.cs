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
        [field: SerializeField] public int NumberOfAttacks { get; private set; }
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

        [HideInInspector] public GameObject model;

        public T GetData<T>()
        {
            return componentDatas.OfType<T>().FirstOrDefault();
        }


        public List<Type> GetAllDependencies()
        {
            return componentDatas.Select(component => component.ComponentDependancy).ToList();
        }

        public void AddData(ComponentData data)
        {
            if (componentDatas.FirstOrDefault(t => t.GetType() == data.GetType()) != null) return;
            
            componentDatas.Add(data);
        }







    }
}
