using Project;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class WeaponGeneratorPopup : EditorWindow
{
    private enum Page
    {
        General,
        WeaponData
    }

    private Page currentPage = Page.General;

    // General Page Variables
    private string weaponName;
    private string weaponDescription;
    private Sprite weaponIcon;
    private GameObject weaponModel;

    // Weapon Data Page Variables
    private int damage;
    private float attackSpeed;
    private float ChargeupSpeed;
    private float attack1Cooldown;
    private float attack2Cooldown;
    private float attack1ManaUse;
    private float attack2ManaUse;
    private GameObject attack1VFX;
    private GameObject attack2VFX;
    private WeaponInputsSO weaponInputs;

    [MenuItem("Tools/Weapon Generator")]
    public static void ShowWindow()
    {
        WeaponGeneratorPopup window = GetWindow<WeaponGeneratorPopup>("Weapon Generator");
        window.minSize = new Vector2(400, 500);
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Generator Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        currentPage = (Page)GUILayout.Toolbar((int)currentPage, new string[] { "Inventory Item", "Weapon Data" });
        EditorGUILayout.Space();

        switch (currentPage)
        {
            case Page.General:
                DrawGeneralPage();
                break;
            case Page.WeaponData:
                DrawWeaponDataPage();
                break;
        }

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Generate Weapon", GUILayout.Height(40)))
        {
            GenerateWeapon();
        }
    }

    private void DrawGeneralPage()
    {
        GUILayout.Label("Inventory Item", EditorStyles.boldLabel);
        weaponName = EditorGUILayout.TextField("Weapon Name", weaponName);
        EditorGUILayout.LabelField("Description");
        weaponDescription = EditorGUILayout.TextArea(weaponDescription, GUILayout.Height(60));
        weaponIcon = (Sprite)EditorGUILayout.ObjectField("Weapon Icon", weaponIcon, typeof(Sprite), false);
        weaponModel = (GameObject)EditorGUILayout.ObjectField("Weapon Model", weaponModel, typeof(GameObject), false);
    }

    private void DrawWeaponDataPage()
    {
        GUILayout.Label("Weapon Stats & Effects", EditorStyles.boldLabel);
        ChargeupSpeed = EditorGUILayout.FloatField("ChargeUpSpeed", ChargeupSpeed);
        attack1Cooldown = EditorGUILayout.FloatField("Attack 1 Cooldown", attack1Cooldown);
        attack2Cooldown = EditorGUILayout.FloatField("Attack 2 Cooldown", attack2Cooldown);
        attack1ManaUse = EditorGUILayout.FloatField("Attack 1 ManaUse", attack1ManaUse);
        attack2ManaUse = EditorGUILayout.FloatField("Attack 2 ManaUse", attack2ManaUse);
        attack1VFX = (GameObject)EditorGUILayout.ObjectField("Attack 1 VFX", attack1VFX, typeof(GameObject), false);
        attack2VFX = (GameObject)EditorGUILayout.ObjectField("Attack 2 VFX", attack2VFX, typeof(GameObject), false);
        
        weaponInputs = (WeaponInputsSO)EditorGUILayout.ObjectField("Weapon Inputs", weaponInputs, typeof(WeaponInputsSO), false);
    }

    private void GenerateWeapon()
    {
        if (string.IsNullOrEmpty(weaponName) || string.IsNullOrEmpty(weaponDescription) || weaponIcon == null || weaponModel == null)
        {
            EditorUtility.DisplayDialog("Error", "Please fill in all required fields on both pages.", "OK");
            return;
        }

        string weaponFolderPath = $"Assets/WeaponSystem/Weapons/{weaponName}";

        if (!AssetDatabase.IsValidFolder("Assets/WeaponSystem"))
        {
            AssetDatabase.CreateFolder("Assets", "WeaponSystem");
        }
        if (!AssetDatabase.IsValidFolder("Assets/WeaponSystem/Weapons"))
        {
            AssetDatabase.CreateFolder("Assets/WeaponSystem", "Weapons");
        }
        if (!AssetDatabase.IsValidFolder(weaponFolderPath))
        {
            AssetDatabase.CreateFolder("Assets/WeaponSystem/Weapons", weaponName);
        }

        string animationsFolder = $"{weaponFolderPath}/Animations";
        string modelsFolder = $"{weaponFolderPath}/Models";

        if (!AssetDatabase.IsValidFolder(animationsFolder))
        {
            AssetDatabase.CreateFolder(weaponFolderPath, "Animations");
        }
        if (!AssetDatabase.IsValidFolder(modelsFolder))
        {
            AssetDatabase.CreateFolder(weaponFolderPath, "Models");
        }

        string sourceAnimatorPath = "Assets/WeaponSystem/Resources/ExampleAnimatorController.controller";
        string destinationAnimatorPath = $"{animationsFolder}/ACont_{weaponName}.controller";
        AnimatorController animatorController = null;
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(sourceAnimatorPath) != null)
        {
            AssetDatabase.CopyAsset(sourceAnimatorPath, destinationAnimatorPath);
            animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(destinationAnimatorPath);
        }

        string modelPath = AssetDatabase.GetAssetPath(weaponModel);
        if (!string.IsNullOrEmpty(modelPath))
        {
            string destinationModelPath = $"{modelsFolder}/{weaponName}_Model.prefab";
            AssetDatabase.MoveAsset(modelPath, destinationModelPath);
        }

        WeaponDataSO weaponData = ScriptableObject.CreateInstance<WeaponDataSO>();

        weaponData.ChargeUpSpeed = ChargeupSpeed;
        weaponData.Inputs = weaponInputs;
        weaponData.AnimController = animatorController;
        weaponData.Attack1Cooldown = attack1Cooldown;
        weaponData.Attack2Cooldown = attack2Cooldown;
        weaponData.Attack1ManaUse = attack1ManaUse;
        weaponData.Attack2ManaUse = attack2ManaUse;
        weaponData.Attack1Type = DamageType.Magic;
        weaponData.Attack2Type = DamageType.Magic;
        weaponData.Attack1VFX = attack1VFX;
        weaponData.Attack2VFX = attack2VFX;




        AssetDatabase.CreateAsset(weaponData, $"Assets/WeaponSystem/Weapons/Datas/WD_{weaponName}.asset");


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = weaponData;

        EditorUtility.DisplayDialog("Done!", "Now remember to fill in your component datas in the weapon Data!", "OK");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        Item item = ScriptableObject.CreateInstance<Item>();
        item.Name = weaponName;
        item.inventorySprite = weaponIcon;
        item.itemTag = SlotTag.Weapon;
        item.weaponData = weaponData;
        item.model = weaponModel;

        AssetDatabase.CreateAsset(item, $"Assets/Items/II_{weaponName}.asset");


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Lock the assemblies to prevent reloading
        EditorUtility.RequestScriptReload();
    }
}