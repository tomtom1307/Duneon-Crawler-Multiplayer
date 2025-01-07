using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Project;

public class AddComponentDataPopup : EditorWindow
{
    private WeaponDataSO dataSO;
    private static List<Type> dataComponentTypes = new List<Type>();

    // Add Popup-related fields
    private string[] options = new string[] { "Attack1", "Attack2", "Generic"};
    private int LeftOrRightClick = 0;  // Default selection

    // The constructor method accepts the target WeaponDataSO
    public static void ShowWindow(WeaponDataSO targetDataSO)
    {
        // Create a window for adding ComponentData
        var window = GetWindow<AddComponentDataPopup>("Add Component Data");
        window.dataSO = targetDataSO;
        window.minSize = new Vector2(250, 300);  // Adjust size to fit both sections
        window.Show();
    }

    private void OnEnable()
    {
        // Load the component types when the window is enabled
        if (dataComponentTypes.Count == 0)
        {
            LoadComponentTypes();
        }
    }

    private void OnGUI()
    {
        if (dataSO == null)
        {
            EditorGUILayout.HelpBox("WeaponDataSO is not assigned.", MessageType.Error);
            return;
        }

        // Section for selecting an attack type
        EditorGUILayout.LabelField("Select Attack Type", EditorStyles.boldLabel);
        LeftOrRightClick = EditorGUILayout.Popup("Select an Option:", LeftOrRightClick, options);

        // Section for displaying buttons for each component type
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Component Data to Add", EditorStyles.boldLabel);

        foreach (var datacompType in dataComponentTypes)
        {
            if (GUILayout.Button(datacompType.Name))
            {
                // Create a new instance of the selected component type
                var comp = Activator.CreateInstance(datacompType) as ComponentData;
                if (comp == null) return;

                // Here we handle the component insertion based on the selected attack option
                if (LeftOrRightClick == 0) // Attack1
                {
                    dataSO.AddDataToAttack1(comp);  // You should define AddDataToAttack1 in WeaponDataSO
                }
                else if (LeftOrRightClick == 1) // Attack2
                {
                    dataSO.AddDataToAttack2(comp);  // Define AddDataToAttack2 in WeaponDataSO
                }

                else if(LeftOrRightClick == 2)
                {
                    dataSO.AddData(comp);
                }

            }
        }
    }

    // Loads all component data types (subclasses of ComponentData)
    private static void LoadComponentTypes()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var filtertypes = types.Where(type => type.IsSubclassOf(typeof(ComponentData)) && !type.ContainsGenericParameters && type.IsClass);
        dataComponentTypes = filtertypes.ToList();
    }
}
