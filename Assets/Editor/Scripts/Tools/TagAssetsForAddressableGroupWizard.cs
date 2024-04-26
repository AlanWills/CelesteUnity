using Celeste;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using CelesteEditor.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Assets
{
    public class TagAssetsForAddressableGroupWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField, AddressableGroup] private string groupName;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Tools/Assets/Tag Assets For Addressable Group")]
        public static void ShowTagAssetsForAddressableGroupWizard()
        {
            DisplayWizard<TagAssetsForAddressableGroupWizard>("Tag Assets For Addressable Group", "Close", "Tag");
        }

        #endregion

        #region Wizard Methods

        protected override bool DrawWizardGUI()
        {
            bool changed = base.DrawWizardGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Assets", CelesteGUIStyles.BoldLabel);

            var selection = Selection.objects;

            if (selection != null && selection.Length > 0)
            {
                for (int i = 0, n = selection.Length; i < n; ++i)
                {
                    EditorGUILayout.LabelField(selection[i].name);
                }
            }
            else
            {
                EditorGUILayout.LabelField("Nothing selected...");
            }

            return changed;
        }

        private void OnWizardOtherButton()
        {
            TagAssets();
        }

        private void OnWizardCreate()
        {
            Close();
        }

        #endregion

        #region Utility

        private bool ShouldBeIncludedInTagging(Object obj)
        {
            return !AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(obj));
        }

        protected void TagAssets()
        {
            List<Object> objectsToTag = new List<Object>();
            List<Object> selectedObjects = new List<Object>(Selection.objects);
            int currentIndex = 0;

            while (currentIndex < selectedObjects.Count)
            {
                var currentObject = selectedObjects[currentIndex];

                if (ShouldBeIncludedInTagging(currentObject))
                {
                    objectsToTag.Add(currentObject);
                }
                else
                {
                    string assetPath = AssetDatabase.GetAssetPath(currentObject);
                    if (AssetDatabase.IsValidFolder(assetPath))
                    {
                        selectedObjects.AddRange(EditorOnly.FindAssets<Object>(string.Empty, assetPath));
                    }
                }

                ++currentIndex;
            }

            for (int i = 0, n = objectsToTag.Count; i < n; ++i)
            {
                objectsToTag[i].SetAddressableInfo(groupName, AssetDatabase.GetAssetPath(objectsToTag[i]));
            }
        }

        #endregion
    }
}
