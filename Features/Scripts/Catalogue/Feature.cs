using Celeste.Logic;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(Feature), menuName = "Celeste/Features/Feature")]
    public class Feature : ScriptableObject, IGuid
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

        public bool IsEnabled
        {
            get { return isEnabled.Value; }
            set { isEnabled.Value = value; }
        }
        
        [SerializeField] private int guid;
        [SerializeField] private BoolValue isEnabled;
        [SerializeField] private Condition canEnable;

        #endregion

        public void Hookup()
        {
            if (canEnable != null)
            {
                canEnable.Init();
                canEnable.ValueChanged.AddListener(OnCanEnableValueChanged);

                if (canEnable.Value)
                {
                    IsEnabled = true;
                }
            }
        }

        public void Shutdown()
        {
            if (canEnable != null)
            {
                canEnable.ValueChanged.RemoveListener(OnCanEnableValueChanged);
                canEnable.Shutdown();
            }
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
            if (!IsEnabled)
            {
                IsEnabled = args.newValue;
            }
        }

        #endregion
    }
}
