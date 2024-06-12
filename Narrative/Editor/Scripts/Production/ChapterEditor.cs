using Celeste.FSM;
using Celeste.FSM.Nodes;
using Celeste.FSM.Nodes.Parameters;
using Celeste.Narrative;
using Celeste.Narrative.Characters;
using Celeste.Parameters;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Production
{
    [CustomEditor(typeof(Chapter))]
    public class ChapterEditor : Editor
    {
        #region Properties and Fields

        private SerializedProperty charactersProperty;
        private SerializedProperty stringValuesProperty;
        private SerializedProperty boolValuesProperty;

        #endregion

        private void OnEnable()
        {
            charactersProperty = serializedObject.FindProperty("characters");
            stringValuesProperty = serializedObject.FindProperty("stringValues");
            boolValuesProperty = serializedObject.FindProperty("boolValues");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Chapter chapter = target as Chapter;

            if (GUILayout.Button("Find Characters"))
            {
                chapter.FindCharacters();
            }

            if (GUILayout.Button("Find Values"))
            {
                FindValues<string, StringValue, StringReference>(stringValuesProperty);
                FindValues<bool, BoolValue, BoolReference>(boolValuesProperty);
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }

        private void FindValues<T, TValue, TReference>(SerializedProperty valuesProperty)
            where TValue : ScriptableObject, IValue<T>
            where TReference : ParameterReference<T, TValue, TReference>
        {
            Chapter chapter = target as Chapter;
            NarrativeGraph narrativeGraph = chapter.NarrativeGraph;
            HashSet<TValue> values = new HashSet<TValue>();

            FindValues<T, TValue, TReference>(valuesProperty, narrativeGraph, values);
        }

        private void FindValues<T, TValue, TReference>(SerializedProperty valuesProperty, FSMGraph fsmGraph, HashSet<TValue> values)
            where TValue : ScriptableObject, IValue<T>
            where TReference : ParameterReference<T, TValue, TReference>
        {
            foreach (FSMNode node in fsmGraph.nodes)
            {
                if (node is SetValueNode<T, TValue, TReference>)
                {
                    var setValueNode = node as SetValueNode<T, TValue, TReference>;
                    if (setValueNode.value != null && !values.Contains(setValueNode.value))
                    {
                        values.Add(setValueNode.value);
                    }
                }
                else if (node is SubFSMNode)
                {
                    SubFSMNode subFSMNode = node as SubFSMNode;

                    FindValues<T, TValue, TReference>(valuesProperty, subFSMNode.SubFSM, values);
                }
            }

            int valueIndex = 0;
            valuesProperty.arraySize = values.Count;

            foreach (TValue value in values)
            {
                valuesProperty.GetArrayElementAtIndex(valueIndex).objectReferenceValue = value;
                ++valueIndex;
            }
        }
    }
}