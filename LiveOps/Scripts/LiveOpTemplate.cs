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
        public long SubType => subType;
        public long StartTimestamp => startTimestamp;
        public bool IsRecurring => isRecurring;
        public long RepeatsAfter => repeatsAfter;
        public int NumComponents => components.Count;

        [SerializeField] private long type;
        [SerializeField] private long subType;
        [SerializeField, Timestamp] private long startTimestamp;
        [SerializeField, Tooltip("The wait time between an event starting and the next recurring event starting in seconds.")] private bool isRecurring;
        [SerializeField, ShowIf(nameof(isRecurring))] private long repeatsAfter;
        [SerializeField] private List<Component> components = new List<Component>();
        [SerializeField] private List<ComponentData> componentsData = new List<ComponentData>();

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
                components.Add(component);
                componentsData.Add(component.CreateData());
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
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
                components.RemoveAt(componentIndex);
                componentsData.RemoveAt(componentIndex);
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    UnityEditor.EditorUtility.SetDirty(this);
                    UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
                }
#endif
            }
        }

        public Component GetComponent(int componentIndex)
        {
            return components.Get(componentIndex);
        }

        public ComponentData GetComponentData(int componentIndex)
        {
            return componentsData.Get(componentIndex);
        }
    }
}
