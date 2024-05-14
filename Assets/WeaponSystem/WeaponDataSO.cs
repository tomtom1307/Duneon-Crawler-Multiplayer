using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    [CreateAssetMenu(menuName = "newWeaponData")]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public int NumberOfAttacks { get; private set; }
        [field: SerializeField] public GameObject Model{ get; private set; }
        [field: SerializeField] public Sprite InventorySprite{ get; private set; }
        [field: SerializeField] public SlotTag itemTag { get; private set; }
        [field: SerializeField] public float Attack1Cooldown { get; private set; }
        [field: SerializeField] public float Attack2Cooldown { get; private set; }


        [field: SerializeReference] public List<ComponentData> componentDatas { get; private set; }


        public T GetData<T>()
        {
            return componentDatas.OfType<T>().FirstOrDefault();
        }

        public void AddData(ComponentData data)
        {
            if (componentDatas.FirstOrDefault(t => t.GetType() == data.GetType()) != null) return;
            
            componentDatas.Add(data);
        }




    }
}
