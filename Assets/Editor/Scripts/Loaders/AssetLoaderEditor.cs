using Celeste.Assets;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Assets
{
    [CustomEditor(typeof(AssetLoader))]
    public class AssetLoaderEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty assetsToLoadProperty;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            assetsToLoadProperty = serializedObject.FindProperty("assetsToLoad");
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find Assets To Load"))
            {
                serializedObject.Update();

                AssetLoader assetLoader = target as AssetLoader;
                List<GameObject> assetsToLoad = new List<GameObject>();

                foreach (GameObject rootGameObject in assetLoader.gameObject.scene.GetRootGameObjects())
                {
                    foreach (IHasAssets hasAssetsToLoad in rootGameObject.GetComponentsInChildren<IHasAssets>())
                    {
                        // This cast is safe because we know we're dealing with a component right now
                        assetsToLoad.Add((hasAssetsToLoad as Component).gameObject);
                    }
                }

                assetsToLoadProperty.arraySize = assetsToLoad.Count;

                for (int i = 0, n = assetsToLoad.Count; i < n; ++i)
                {
                    assetsToLoadProperty.GetArrayElementAtIndex(i).objectReferenceValue = assetsToLoad[i];
                }

                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }

        #endregion
    }
}
