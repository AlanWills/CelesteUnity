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

                foreach (IHasAssets hasAssetsToLoad in assetLoader.GetComponentsInChildren<IHasAssets>())
                {
                    assetsToLoad.Add(hasAssetsToLoad.gameObject);
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