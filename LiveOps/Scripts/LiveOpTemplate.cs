using Celeste.Components;
using Celeste.DataStructures;
using Celeste.Tools.Attributes.GUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Component = Celeste.Components.Component;

namespace Celeste.LiveOps
{
    [CreateAssetMenu(fileName = nameof(LiveOpTemplate), menuName = "Celeste/Live Ops/Live Op Template")]
    public class LiveOpTemplate : ScriptableObject
    {
        #region Properties and Fields

        public long Type => type;
        public long StartTimestamp => startTimestamp;
        public int NumComponents => components.Count;

        [SerializeField] private long type;
        [Timestamp, SerializeField] private long startTimestamp;
        [SerializeField] private List<ComponentTemplate> components = new List<ComponentTemplate>();

        #endregion

        public void AddComponent(Type type)
        {
            Component component = CreateInstance(type) as Component;
#if NULL_CHECKS
            UnityEngine.Debug.Assert(component != null, $"Null component added to {name}.");
            if (component != null)
#endif
            {
                component.name = type.Name;
                component.hideFlags = HideFlags.HideInHierarchy;

                ComponentTemplate componentTemplate = CreateInstance<ComponentTemplate>();
                componentTemplate.name = type.Name;
                componentTemplate.component = component;
                componentTemplate.componentData = component.CreateData();

                components.Add(componentTemplate);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.AssetDatabase.AddObjectToAsset(componentTemplate, this);
                    UnityEditor.AssetDatabase.AddObjectToAsset(component, this);
                    UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
                }
#endif
            }
        }

        public void RemoveComponent(int componentIndex)
        {
#if INDEX_CHECKS
            if (0 <= componentIndex && componentIndex < components.Count)
#endif
            {
                ComponentTemplate componentTemplate = components[componentIndex];
                components.RemoveAt(componentIndex);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DestroyImmediate(componentTemplate.component, true);
                    DestroyImmediate(componentTemplate, true);
                    UnityEditor.EditorUtility.SetDirty(this);
                    UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
                }
#endif
            }
        }

        public ComponentTemplate GetComponent(int componentIndex)
        {
            return components.Get(componentIndex);
        }
    }
}
