using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.UI
{
    [CustomEditor(typeof(NarrativeViewManager))]
    public class NarrativeViewManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find All", GUILayout.ExpandWidth(false)))
            {
                serializedObject.Update();

                NarrativeViewManager narrativeViewManager = target as NarrativeViewManager;
                SerializedProperty narrativeViews = serializedObject.FindProperty("narrativeViews");
                var foundNarrativeViews = narrativeViewManager.GetComponentsInChildren<NarrativeView>();
                narrativeViews.arraySize = foundNarrativeViews.Length;

                for (int i = 0, n = foundNarrativeViews.Length; i < n; ++i)
                {
                    narrativeViews.GetArrayElementAtIndex(i).objectReferenceValue = foundNarrativeViews[i];
                }

                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }
    }
}
