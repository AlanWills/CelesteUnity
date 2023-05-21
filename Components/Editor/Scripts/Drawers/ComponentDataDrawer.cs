using Celeste.DataStructures;
using System;
using UnityEditor;

namespace CelesteEditor.Components
{
    public class CustomComponentDataDrawerAttribute : Attribute
    {
        public Type ComponentType { get; }

        public CustomComponentDataDrawerAttribute(Type componentType)
        {
            ComponentType = componentType;
        }
    }

    public class ComponentDataDrawer
    {
        #region Properties and Fields

        protected SerializedProperty dataProperty;
        protected SerializedObject componentObject;

        #endregion

        public void Enable(SerializedProperty dataProperty, SerializedObject componentObject)
        {
            this.dataProperty = dataProperty;
            this.componentObject = componentObject;

            OnEnable();
        }

        protected virtual void OnEnable() { }

        public void InspectorGUI()
        {
            componentObject.Update();

            OnInspectorGUI();
        }

        protected virtual void OnInspectorGUI()
        {
            DrawPropertiesExcluding();
        }

        protected void DrawPropertiesExcluding(params string[] properties)
        {
            SerializedProperty p = dataProperty.Copy();
            p.NextVisible(true);

            do
            {
                if (!properties.Contains(p.name))
                {
                    EditorGUILayout.PropertyField(p);
                }
            }
            while (p.NextVisible(true));
        }
    }
}
