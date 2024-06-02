using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerUsingTemplates<TComponent> : ScriptableObject, IComponentContainerUsingTemplates<TComponent> 
        where TComponent : BaseComponent
    {
        #region Template

        [Serializable]
        private struct Template
        {
            public TComponent component;
            [SerializeReference] public ComponentData data;
        }

        #endregion

        #region Properties and Fields

        public int NumComponents => componentTemplates.Count;

        [SerializeField] private List<Template> componentTemplates = new List<Template>();

        #endregion

        public void AddEmptyTemplate()
        {
            componentTemplates.Add(new Template());
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public TComponent GetComponent(int index)
        {
            return componentTemplates.Get(index).component;
        }

        public ComponentData GetComponentData(int index)
        {
            return componentTemplates.Get(index).data;
        }

        public void SetComponentData(int index, ComponentData componentData)
        {
            if (index < NumComponents)
            {
                var template = componentTemplates[index];
                template.data = componentData;
                componentTemplates[index] = template;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public bool HasComponent<K>() where K : TComponent
        {
            return componentTemplates.Exists(x => x is K);
        }

        public void RemoveComponent(int componentIndex)
        {
            componentTemplates.RemoveAt(componentIndex);
        }
    }
}
