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
            if (GUILayout.Button("Find All In Scene", GUILayout.ExpandWidth(false)))
            {
                serializedObject.Update();

                NarrativeViewManager narrativeViewManager = target as NarrativeViewManager;
                SerializedProperty narrativeViews = serializedObject.FindProperty("narrativeViews");
                int startingSize = 0;
                narrativeViews.arraySize = startingSize;
                
                foreach (GameObject rootGameObject in narrativeViewManager.gameObject.scene.GetRootGameObjects())
                {
                    var foundNarrativeViews = rootGameObject.GetComponentsInChildren<NarrativeView>();
                    int foundNarrativeViewsCount = foundNarrativeViews.Length;

                    if (foundNarrativeViewsCount <= 0)
                    {
                        continue;
                    }
                    
                    narrativeViews.arraySize += foundNarrativeViewsCount;

                    for (int i = 0; i < foundNarrativeViewsCount; ++i)
                    {
                        narrativeViews.GetArrayElementAtIndex(startingSize + i).objectReferenceValue = foundNarrativeViews[i];
                    }
                    
                    startingSize += foundNarrativeViewsCount;
                }

                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }
    }
}
