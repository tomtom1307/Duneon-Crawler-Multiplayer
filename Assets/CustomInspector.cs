
using UnityEditor;
using UnityEngine;

namespace Project
{
    [CustomEditor(typeof(GenerateButton))]
    public class CustomInspector : Editor
    {
        public override void OnInspectorGUI ()
        {
            DrawDefaultInspector();

            GenerateButton gen = (GenerateButton)target;
            if(GUILayout.Button("Generate Dungeon"))
            {
                gen.DoStuff();
            }
        }
    }
}
