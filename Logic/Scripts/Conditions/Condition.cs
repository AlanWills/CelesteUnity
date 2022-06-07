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
        #region Properties and Fields

        public UnityEvent<ValueChangedArgs<bool>> ValueChanged { get; } = new UnityEvent<ValueChangedArgs<bool>>();

        public virtual bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    ValueChanged.Invoke(new ValueChangedArgs<bool>(!value, value));
                }
            }
        }

        [NonSerialized] private bool _value = false;

        #endregion

        public void Init()
        {
            DoInit();
            Check();
        }

        public void Shutdown()
        {
            ValueChanged.RemoveAllListeners();
            DoShutdown();
        }

        protected bool Check()
        {
            Value = DoCheck();
            return Value;
        }

        protected virtual void DoInit() { }
        protected virtual void DoShutdown() { }
        protected abstract bool DoCheck();
        public abstract void SetTarget(object arg);

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
