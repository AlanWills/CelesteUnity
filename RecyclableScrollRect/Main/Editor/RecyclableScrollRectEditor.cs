﻿//MIT License
//Copyright (c) 2020 Mohammed Iqubal Hussain
//Website : Polyandcode.com 

using UnityEngine.UI;
using UnityEditor.AnimatedValues;
using UnityEditor;
using UnityEngine;
using static PolyAndCode.UI.RecyclableScrollRect;

namespace PolyAndCode.UI
{
    [CustomEditor(typeof(RecyclableScrollRect), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom Editor for the Recyclable Scroll Rect Component which is derived from Scroll Rect.
    /// </summary>

    public class RecyclableScrollRectEditor : Editor
    {
        SerializedProperty m_Content;
        SerializedProperty m_MovementType;
        SerializedProperty m_Elasticity;
        SerializedProperty m_Inertia;
        SerializedProperty m_DecelerationRate;
        SerializedProperty m_ScrollSensitivity;
        SerializedProperty m_Viewport;
        SerializedProperty m_OnValueChanged;

        //inherited
        SerializedProperty _protoTypeCell;
        SerializedProperty _selfInitialize;
        SerializedProperty _direction;
        SerializedProperty _verticalDirection;
        SerializedProperty _type;
        SerializedProperty _portraitModeSegments;
        SerializedProperty _landscapeModeSegments;

        AnimBool m_ShowElasticity;
        AnimBool m_ShowDecelerationRate;

        RecyclableScrollRect _script;
        protected virtual void OnEnable()
        {
            _script = (RecyclableScrollRect)target;

            m_Content = serializedObject.FindProperty("m_Content");
            m_MovementType = serializedObject.FindProperty("m_MovementType");
            m_Elasticity = serializedObject.FindProperty("m_Elasticity");
            m_Inertia = serializedObject.FindProperty("m_Inertia");
            m_DecelerationRate = serializedObject.FindProperty("m_DecelerationRate");
            m_ScrollSensitivity = serializedObject.FindProperty("m_ScrollSensitivity");
            m_Viewport = serializedObject.FindProperty("m_Viewport");
            m_OnValueChanged = serializedObject.FindProperty("m_OnValueChanged");

            //Inherited
            _protoTypeCell = serializedObject.FindProperty("PrototypeCell");
            _selfInitialize = serializedObject.FindProperty("SelfInitialize");
            _direction = serializedObject.FindProperty("Direction");
            _verticalDirection = serializedObject.FindProperty(nameof(RecyclableScrollRect.VerticalDirection));
            _type = serializedObject.FindProperty("IsGrid");
            _portraitModeSegments = serializedObject.FindProperty(nameof(_portraitModeSegments));
            _landscapeModeSegments = serializedObject.FindProperty(nameof(_landscapeModeSegments));

            m_ShowElasticity = new AnimBool(Repaint);
            m_ShowDecelerationRate = new AnimBool(Repaint);
            SetAnimBools(true);
        }

        protected virtual void OnDisable()
        {
            m_ShowElasticity.valueChanged.RemoveListener(Repaint);
            m_ShowDecelerationRate.valueChanged.RemoveListener(Repaint);
        }

        void SetAnimBools(bool instant)
        {
            SetAnimBool(m_ShowElasticity, !m_MovementType.hasMultipleDifferentValues && m_MovementType.enumValueIndex == (int)ScrollRect.MovementType.Elastic, instant);
            SetAnimBool(m_ShowDecelerationRate, !m_Inertia.hasMultipleDifferentValues && m_Inertia.boolValue, instant);
        }

        void SetAnimBool(AnimBool a, bool value, bool instant)
        {
            if (instant)
                a.value = value;
            else
                a.target = value;
        }

        public override void OnInspectorGUI()
        {
            SetAnimBools(false); 
            serializedObject.Update();
          
            EditorGUILayout.PropertyField(_direction);

            if (_direction.enumValueIndex == (int)DirectionType.Vertical)
            {
                EditorGUILayout.PropertyField(_verticalDirection);
            }

            EditorGUILayout.PropertyField(_type, new GUIContent("Grid"));
            if (_type.boolValue)
            {
                string title = _direction.enumValueIndex == (int)RecyclableScrollRect.DirectionType.Vertical ? "Columns" : "Rows";
                EditorGUILayout.PropertyField(_portraitModeSegments, new GUIContent($"{title} in Portrait"));
                EditorGUILayout.PropertyField(_landscapeModeSegments, new GUIContent($"{title} in Landscape"));
                EditorUtility.SetDirty(_script);
            }

            EditorGUILayout.PropertyField(_selfInitialize);
            EditorGUILayout.PropertyField(m_Viewport);
            EditorGUILayout.PropertyField(m_Content);
            EditorGUILayout.PropertyField(_protoTypeCell);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_MovementType);
            if (EditorGUILayout.BeginFadeGroup(m_ShowElasticity.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_Elasticity);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(m_Inertia);
            if (EditorGUILayout.BeginFadeGroup(m_ShowDecelerationRate.faded))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_DecelerationRate);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();

            EditorGUILayout.PropertyField(m_ScrollSensitivity);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_OnValueChanged);

            serializedObject.ApplyModifiedProperties();
        }
    }
}