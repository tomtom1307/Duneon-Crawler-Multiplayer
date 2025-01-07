using Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(WeaponDataSO))]
public class WeaponDataSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Add Component Data"))
        {
            // Open the popup window with the current WeaponDataSO instance
            AddComponentDataPopup.ShowWindow((WeaponDataSO)target);
        }
    }
}



    /*
    //Only 1 can exist
    private static List<Type> dataComponentTypes = new List<Type>();
    private WeaponDataSO dataSO;



    private void OnEnable()
    {
        dataSO = target as WeaponDataSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var datacompType in dataComponentTypes)
        {
            if (GUILayout.Button(datacompType.Name))
            {
                var comp = Activator.CreateInstance(datacompType) as ComponentData;

                if(comp == null)
                {
                    return;
                }

                dataSO.AddData(comp);
            }
        }

    }

    [DidReloadScripts]
    private static void OnRecompile()
    {
        //Get a list of all assemblies currently loaded into the app (Assemblies - Unit of code that complied+Executed in the .NET runtime// Types and classes n shit)
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //Loop through assemblies and extract type info
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());

        //Look for types we care about in our case ComponentData type
        //In the structure of the weapon system component data also has a generic type which we dont want to consider
        var filtertypes = types.Where(
            type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass);

        //Store into DataComptypes
        dataComponentTypes = filtertypes.ToList();
            
        


    }
    */

