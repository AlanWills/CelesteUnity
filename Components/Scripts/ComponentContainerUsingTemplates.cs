using Celeste.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using Celeste.Tools;
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
            EditorOnly.SetDirty(this);
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
                EditorOnly.SetDirty(this);
            }
        }

        public bool HasComponent<K>() where K : TComponent
        {
            return componentTemplates.Exists(x => x.component is K);
        }

        public K FindComponent<K>() where K : TComponent
        {
            var componentTemplate = componentTemplates.FirstOrDefault(x => x.component is K);
            return componentTemplate.component as K;
        }

        public void RemoveComponent(int componentIndex)
        {
            componentTemplates.RemoveAt(componentIndex);
        }
    }
}
