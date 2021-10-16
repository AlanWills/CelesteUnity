using Celeste.FSM;
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
                NarrativeGraph narrativeGraph = chapter.NarrativeGraph;
                HashSet<Character> characters = new HashSet<Character>();

                foreach (FSMNode node in narrativeGraph.nodes)
                {
                    if (node is ICharacterNode)
                    {
                        ICharacterNode characterNode = node as ICharacterNode;

                        if (characterNode.Character != null && !characters.Contains(characterNode.Character))
                        {
                            characters.Add(characterNode.Character);
                        }
                    }
                }

                int characterIndex = 0;
                charactersProperty.arraySize = characters.Count;

                foreach (Character character in characters)
                {
                    charactersProperty.GetArrayElementAtIndex(characterIndex).objectReferenceValue = character;
                    ++characterIndex;
                }
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
            where TValue : ParameterValue<T>
            where TReference : ParameterReference<T, TValue, TReference>
        {
            Chapter chapter = target as Chapter;
            NarrativeGraph narrativeGraph = chapter.NarrativeGraph;
            HashSet<TValue> values = new HashSet<TValue>();

            foreach (FSMNode node in narrativeGraph.nodes)
            {
                if (node is SetValueNode<T, TValue, TReference>)
                {
                    var setValueNode = node as SetValueNode<T, TValue, TReference>;
                    if (setValueNode.value != null && !values.Contains(setValueNode.value))
                    {
                        values.Add(setValueNode.value);
                    }
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