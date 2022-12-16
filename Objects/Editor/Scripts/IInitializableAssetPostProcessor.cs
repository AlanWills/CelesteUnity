using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Objects
{
    public class IInitializableAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (int i = 0, n = importedAssets != null ? importedAssets.Length : 0; i < n; i++)
            {
                IInitializable initializable = AssetDatabase.LoadAssetAtPath<ScriptableObject>(importedAssets[i]) as IInitializable;
                
                if (initializable != null)
                {
                    initializable.Initialize();
                }
            }
        }
    }
}
