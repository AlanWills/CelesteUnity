using Celeste.Components;
using System;
using UnityEditor;

namespace CelesteEditor.Components
{
    public abstract class ComponentContainerUsingSubAssetsEditor<TComponent> : Editor where TComponent : BaseComponent
    {
        #region Properties and Fields

        protected abstract Type[] AllComponentTypes { get; }
        protected abstract string[] AllComponentDisplayNames { get; }

        private ComponentContainerUsingSubAssets<TComponent> ComponentContainer => target as ComponentContainerUsingSubAssets<TComponent>;

        private SerializedProperty componentsProperty;
        private int selectedTypeIndex = 0;

        #endregion

        private void OnEnable()
        {
            componentsProperty = serializedObject.FindProperty("components");
        }

        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "components");

            selectedTypeIndex = CelesteEditorGUILayout.SubAssetListField(
                "Components",
                AllComponentTypes,
                AllComponentDisplayNames,
                selectedTypeIndex,
                componentsProperty,
                (t) => ComponentContainer.CreateComponent(t),
                ComponentContainer.RemoveComponent);

            DrawInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspectorGUI() { }

        #endregion
    }
}
