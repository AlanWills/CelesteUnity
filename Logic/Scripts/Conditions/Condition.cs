using Celeste.Events;
using Celeste.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Logic
{
    [Serializable]
    public abstract class Condition : ScriptableObject, ICopyable<Condition>
    {
        #region Properties and Fields

        private BoolValueChangedUnityEvent onIsMetChangedUnityEvent;
        private BoolValueChangedUnityEvent OnIsMetChangedUnityEvent
        {
            get
            {
                if (onIsMetChangedUnityEvent == null)
                {
                    onIsMetChangedUnityEvent = new BoolValueChangedUnityEvent();
                }

                return onIsMetChangedUnityEvent;
            }
        }

        public virtual bool IsMet
        {
            get => isMet;
            set
            {
                if (isMet != value)
                {
                    isMet = value;

                    InvokeValueChanged(new ValueChangedArgs<bool>(!value, value));
                }
            }
        }

        [SerializeField] private BoolValueChangedEvent onIsMetChanged;

        [NonSerialized] private bool isMet = false;
        [NonSerialized] private int initCount = 0;

        #endregion

        public void Init()
        {
            if (initCount++ == 0)
            {
                DoInit();
                Check();
            }
        }

        public void Shutdown()
        {
            Debug.Assert(initCount > 0, $"Condition {name} {nameof(Shutdown)} called more times than {nameof(Init)}.");

            if (--initCount == 0)
            {
                RemoveAllListeners();
                DoShutdown();
            }
        }

        protected bool Check()
        {
            IsMet = DoCheck();
            return IsMet;
        }

        protected virtual void DoInit() { }
        protected virtual void DoShutdown() { }
        protected abstract bool DoCheck();
        public abstract void SetTarget(object arg);

        #region ICopyable

        public abstract void CopyFrom(Condition original);

        #endregion

        public void AddOnIsMetConditionChanged(UnityAction<ValueChangedArgs<bool>> onIsMetConditionChanged)
        {
            if (onIsMetChanged != null)
            {
                onIsMetChanged.AddListener(onIsMetConditionChanged);
            }
            else
            {
                OnIsMetChangedUnityEvent.AddListener(onIsMetConditionChanged);
            }
        }

        public void RemoveOnIsMetConditionChanged(UnityAction<ValueChangedArgs<bool>> onIsMetConditionChanged)
        {
            if (onIsMetChanged != null)
            {
                onIsMetChanged.RemoveListener(onIsMetConditionChanged);
            }
            else
            {
                OnIsMetChangedUnityEvent.RemoveListener(onIsMetConditionChanged);
            }
        }

        private void InvokeValueChanged(ValueChangedArgs<bool> valueChangedArgs)
        {
            if (onIsMetChanged != null)
            {
                onIsMetChanged.Invoke(valueChangedArgs);
            }
            else
            {
                OnIsMetChangedUnityEvent.Invoke(valueChangedArgs);
            }
        }

        private void RemoveAllListeners()
        {
            if (onIsMetChanged != null)
            {
                onIsMetChanged.RemoveAllListeners();
            }
            else
            {
                OnIsMetChangedUnityEvent.RemoveAllListeners();
            }
        }

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
