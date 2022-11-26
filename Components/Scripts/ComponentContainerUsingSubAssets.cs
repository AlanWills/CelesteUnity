using Celeste.DataStructures;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerUsingSubAssets<T> : ScriptableObject, IComponentContainer<T> where T : Component
    {
        #region Properties and Fields

        public int NumComponents => components.Count;

        [SerializeField] private List<T> components = new List<T>();

        #endregion

        public void CreateComponent<K>() where K : T
        {
            CreateComponent(typeof(K));
        }

        public void CreateComponent(Type type)
        {
            this.AddSubAsset(type, components);
        }

        public T GetComponent(int index)
        {
            return components.Get(index);
        }

        public bool HasComponent<K>() where K : T
        {
            return components.Exists(x => x is K);
        }

        public void RemoveComponent(int componentIndex)
        {
            this.RemoveSubAsset(componentIndex, components);
        }
    }
}
