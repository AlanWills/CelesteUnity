using Celeste.Input;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;
using Celeste.Parameters.Input;

namespace CelesteEditor.Input
{
    [CustomEditor(typeof(Shortcut))]
    public class ShortcutEditor : Editor
    {
        #region Create Menu Item
        
        [MenuItem("Assets/Create/Celeste/Input/Shortcut")]
        public static void CreateMenuItem()
        {
            Shortcut shortcut = CreateInstance<Shortcut>();
            shortcut.name = "New Shortcut";

            // Create the asset first, so it will be persistent when we create the sub assets
            EditorOnly.CreateAssetInFolderAndSave(shortcut, EditorOnly.GetSelectionObjectPath());

            shortcut.key = CreateInstance<KeyReference>();
            shortcut.key.name = "KeyCodeReference";
            shortcut.key.hideFlags = HideFlags.HideInHierarchy;
            AssetDatabase.AddObjectToAsset(shortcut.key, shortcut);

            shortcut.fired = CreateInstance<Celeste.Events.Event>();
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
