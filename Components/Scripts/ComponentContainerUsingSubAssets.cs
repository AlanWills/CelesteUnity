using Celeste.DataStructures;
using Celeste.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Components
{
    public class ComponentContainerUsingSubAssets<T> : ScriptableObject, IComponentContainerUsingSubAssets<T> where T : BaseComponent
    {
        #region Properties and Fields

        public int NumComponents => components.Count;

        [SerializeField] private List<T> components = new List<T>();

        #endregion

        public K CreateComponent<K>() where K : T
        {
            return CreateComponent(typeof(K)) as K;
        }

        public T CreateComponent(Type type)
        {
            return this.AddSubAsset(type, components);
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
