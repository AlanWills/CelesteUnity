using Celeste.FSM.Nodes;
using Celeste.FSM.Nodes.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using XNodeEditor;

namespace CelesteEditor.FSM.Nodes.Loading
{
    [CustomNodeEditor(typeof(InstantiatePrefabNode))]
    public class InstantiatePrefabNodeEditor : FSMNodeEditor
    {
        #region Serialized Properties

        private SerializedProperty isAddressableProperty;
        private SerializedProperty addressablePathProperty;
        private SerializedProperty prefabProperty;
        private SerializedProperty parentProperty;

        #endregion

        #region Unity Methods

        public override void OnCreate()
        {
            base.OnCreate();

            isAddressableProperty = serializedObject.FindProperty("isAddressable");
            addressablePathProperty = serializedObject.FindProperty("addressablePath");
            prefabProperty = serializedObject.FindProperty("prefab");
            parentProperty = serializedObject.FindProperty("parent");
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DrawDefaultPortPair();

            EditorGUILayout.PropertyField(isAddressableProperty);

            if (isAddressableProperty.boolValue)
            {
                EditorGUILayout.PropertyField(addressablePathProperty);
                prefabProperty.objectReferenceValue = null;
            }
            else
            {
                EditorGUILayout.PropertyField(prefabProperty);
            }

            EditorGUILayout.PropertyField(parentProperty);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
