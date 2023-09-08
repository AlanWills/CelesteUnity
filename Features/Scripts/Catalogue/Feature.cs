using Celeste.Events;
using Celeste.Logic;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(Feature), menuName = "Celeste/Features/Feature")]
    public class Feature : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public bool IsKilled { get; private set; }

        public bool IsEnabled
        {
            get { return !IsKilled && isEnabled.Value; }
            set { isEnabled.Value = !IsKilled && value; }
        }
        
        [SerializeField] private int guid;
        [SerializeField] private BoolValue isEnabled;
        [SerializeField] private Condition canEnable;

        #endregion

        public void Hookup()
        {
            if (canEnable != null)
            {
                canEnable.AddOnIsMetConditionChanged(OnCanEnableValueChanged);

                if (canEnable.IsMet)
                {
                    IsEnabled = true;
                }
            }
        }

        public void Shutdown()
        {
            if (canEnable != null)
            {
                canEnable.RemoveOnIsMetConditionChanged(OnCanEnableValueChanged);
            }
        }

        public void Kill()
        {
            IsKilled = true;
            IsEnabled = false;
        }

        public void AddOnEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            isEnabled.AddValueChangedCallback(callback);
        }

        public void RemoveOnEnabledChangedCallback(UnityAction<ValueChangedArgs<bool>> callback)
        {
            isEnabled.RemoveValueChangedCallback(callback);
        }

        #region Callbacks

        private void OnCanEnableValueChanged(ValueChangedArgs<bool> args)
        {
            IsEnabled = args.newValue;
        }

        #endregion
    }
}
