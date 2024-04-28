using Celeste.Events;
using Celeste.Objects;
using Celeste.Tools;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Logic
{
    [Serializable]
    public abstract class Condition : ScriptableObject, ICopyable<Condition>, IEditorInitializable
    {
        #region Properties and Fields

        public virtual bool IsMet
        {
            get => isMet;
            set
            {
                if (negate)
                {
                    value = !value;
                }

                if (isMet != value)
                {
                    isMet = value;

                    if (Application.isPlaying)
                    {
                        onIsMetChanged.Invoke(new ValueChangedArgs<bool>(!value, value));
                    }
                }
            }
        }

        [SerializeField] private bool negate = false;
        [SerializeField] private GuaranteedBoolValueChangedEvent onIsMetChanged = new GuaranteedBoolValueChangedEvent();

        [NonSerialized] private bool isMet = false;
        [NonSerialized] private int initCount = 0;

        #endregion

        public void Initialize()
        {
            if (initCount++ == 0)
            {
                DoInitialize();
                Check();
            }
        }

        public void Shutdown()
        {
            UnityEngine.Debug.Assert(initCount > 0, $"Condition {name} {nameof(Shutdown)} called more times than {nameof(Initialize)}.");

            if (--initCount == 0)
            {
                RemoveAllListeners();
                DoShutdown();
            }
        }

        public void SetVariable(object arg)
        {
            DoSetVariable(arg);
            Check();
        }

        public bool Check()
        {
            IsMet = DoCheck();
            return IsMet;
        }

        protected virtual void DoInitialize() { }
        protected virtual void DoShutdown() { }
        protected abstract bool DoCheck();
        protected abstract void DoSetVariable(object arg);

        #region ICopyable

        public abstract void CopyFrom(Condition original);

        #endregion

        public void AddOnIsMetConditionChanged(UnityAction<ValueChangedArgs<bool>> onIsMetConditionChanged)
        {
            onIsMetChanged.AddListener(onIsMetConditionChanged);
        }

        public void RemoveOnIsMetConditionChanged(UnityAction<ValueChangedArgs<bool>> onIsMetConditionChanged)
        {
            onIsMetChanged.RemoveListener(onIsMetConditionChanged);
        }

        private void InvokeValueChanged(ValueChangedArgs<bool> valueChangedArgs)
        {
            onIsMetChanged.Invoke(valueChangedArgs);
        }

        private void RemoveAllListeners()
        {
            onIsMetChanged.RemoveAllListeners();
        }

        protected T CreateDependentAsset<T>(string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            asset.name = name;
            asset.hideFlags = HideFlags.HideInHierarchy;

            EditorOnly.AddObjectToAsset(asset, this);

            return asset;
        }

        public void Editor_Initialize()
        {
            DoEditorInitialize();
        }

        [Conditional("UNITY_EDITOR")]
        protected virtual void DoEditorInitialize()
        {
            DoInitialize();
        }
    }
}
