using Celeste.Objects;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Logic
{
    [Serializable]
    public abstract class Condition : ScriptableObject, ICopyable<Condition>
    {
        public abstract UnityEvent<ValueChangedArgs<bool>> ValueChanged { get; }

        public abstract void Init();
        public abstract void SetTarget(object arg);
        public abstract bool Check();

        #region ICopyable

        public abstract void CopyFrom(Condition original);

        #endregion

        protected T CreateDependentAsset<T>(string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            asset.name = name;
            asset.hideFlags = HideFlags.HideInHierarchy;

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(asset, this);
#endif

            return asset;
        }
    }
}
