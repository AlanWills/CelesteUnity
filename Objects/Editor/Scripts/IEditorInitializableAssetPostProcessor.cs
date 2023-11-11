using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Objects
{
    public class IEditorInitializableAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0, n = importedAssets != null ? importedAssets.Length : 0; i < n; i++)
            {
                IEditorInitializable initializable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(importedAssets[i]) as IEditorInitializable;
                
                if (initializable != null)
                {
                    initializable.Editor_Initialize();
                }
            }
        }
    }
}
