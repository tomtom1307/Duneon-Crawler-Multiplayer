using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    [Serializable]
    public class ComponentData
    {
        [SerializeField, HideInInspector] private string name;

        public Type ComponentDependancy {  get; protected set; }


        public ComponentData() {
            SetComponentName();
        }

        private void SetComponentName() => name  = GetType().Name;

    }
    [Serializable]
    public class ComponentData<T> : ComponentData where T : AttackData
    {
        [field: SerializeField] public T[] AttackData { get; private set; }

    }

}
