﻿using Celeste.DataStructures;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerUsingTemplates<TComponent> : ScriptableObject, IComponentContainer<TComponent> 
        where TComponent : Component
    {
        #region Template

        [Serializable]
        private struct Template
        {
            public TComponent component;
            [SerializeReference, InlineDataInInspector] public ComponentData data;
        }

        #endregion

        #region Properties and Fields

        public int NumComponents => componentTemplates.Count;

        [SerializeField] private List<Template> componentTemplates = new List<Template>();

        #endregion

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