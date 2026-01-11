using Celeste.Events;
using Celeste.Logic;
using Celeste.Objects;
using Celeste.Parameters;
using System;
using Celeste.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(Feature), menuName = CelesteMenuItemConstants.FEATURES_MENU_ITEM + "Feature", order = CelesteMenuItemConstants.FEATURES_MENU_ITEM_PRIORITY)]
    public class Feature : ScriptableObject, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set
            {
                guid = value;
                EditorOnly.SetDirty(this);
            }
        }

        public bool IsKilled { get; private set; }

        public bool IsEnabled
        {
            get => !IsKilled && isEnabled.Value;
            set => isEnabled.Value = value;
        }
        
        [SerializeField] private int guid;
        [SerializeField] private BoolValue isEnabled;
        [SerializeField] private Condition canEnable;

        #endregion

        public void Initialize()
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

        public void Revive()
        {
            IsKilled = false;
            IsEnabled = canEnable?.IsMet ?? true;
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
