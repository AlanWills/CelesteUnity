using Celeste.Input;
using Celeste.Parameters.Input;
using CelesteEditor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Input
{
    [CustomEditor(typeof(Shortcut))]
    public class ShortcutEditor : Editor
    {
        #region Create Menu Item
        
        [MenuItem("Assets/Create/Celeste/Input/Shortcut")]
        public static void CreateMenuItem()
        {
            Shortcut shortcut = ScriptableObject.CreateInstance<Shortcut>();
            shortcut.name = "New Shortcut";

            // Create the asset first, so it will be persistent when we create the sub assets
            AssetUtility.CreateAsset(shortcut, AssetUtility.GetSelectionObjectPath());

            shortcut.keyCode = ScriptableObject.CreateInstance<KeyCodeReference>();
            shortcut.keyCode.name = "KeyCodeReference";
            shortcut.keyCode.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(shortcut.keyCode, shortcut);

            shortcut.fired = ScriptableObject.CreateInstance<Celeste.Events.Event>();
            shortcut.fired.name = "FiredEvent";
            shortcut.fired.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(shortcut.fired, shortcut);

            EditorUtility.SetDirty(shortcut);
            AssetDatabase.SaveAssets();
        }

        #endregion

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Invoke"))
            {
                (target as Shortcut).fired.Invoke();
            }
        }
    }
}
