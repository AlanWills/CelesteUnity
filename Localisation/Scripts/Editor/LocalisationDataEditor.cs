using Celeste.Localisation;
using CelesteEditor.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Localisation
{
    [CustomEditor(typeof(LocalisationData))]
    public class LocalisationDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (GUILayout.Button("Find Entries", GUILayout.ExpandWidth(false)))
            {
                AssetUtility.FindAssets<LocalisationEntry>(target, "localisationEntries");
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
