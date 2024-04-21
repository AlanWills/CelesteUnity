using Celeste.Application;
using System.Collections.Generic;
using System;
using UnityEngine;
using Celeste.Parameters;
using Celeste.Events;
using UnityEngine.Events;

namespace Celeste.Options
{
    [Serializable]
    public struct DefaultValue<T>
    {
        public ApplicationPlatform platform;
        public T value;
    }

    public class Option<T,TValue> : ScriptableObject where TValue : IValue<T>
    {
        #region Properties and Fields

        public string DisplayName => displayName;

        public T Value
        {
            get => value.Value;
            set => this.value.Value = value;
        }

        [SerializeField] private string displayName;
        [SerializeField] private TValue value;
        [SerializeField] private List<DefaultValue<T>> platformDefaultValues = new List<DefaultValue<T>>();

        #endregion

        public T GetDefaultValue(ApplicationPlatform platform)
        {
            int defaultValueIndex = platformDefaultValues.FindIndex(x => x.platform == platform);
            return defaultValueIndex >= 0 ? platformDefaultValues[defaultValueIndex].value : value.DefaultValue;
        }

        public void SetDefaultValue(ApplicationPlatform platform)
        {
            int defaultValueIndex = platformDefaultValues.FindIndex(x => x.platform == platform);
            if (defaultValueIndex >= 0)
            {
                value.Value = platformDefaultValues[defaultValueIndex].value;
            }
        }

        public void AddValueChangedCallback(UnityAction<ValueChangedArgs<T>> onValueChanged)
        {
            value.AddValueChangedCallback(onValueChanged);
        }

        public void RemoveValueChangedCallback(UnityAction<ValueChangedArgs<T>> onValueChanged)
        {
            value.RemoveValueChangedCallback(onValueChanged);
        }
    }
}
