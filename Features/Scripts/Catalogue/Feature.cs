using Celeste.Events;
using Celeste.Objects;
using System;
using UnityEngine;

namespace Celeste.Features
{
    [CreateAssetMenu(fileName = nameof(Feature), menuName = "Celeste/Features/Feature")]
    public class Feature : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid => guid;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    enabledChanged.Invoke(isEnabled);
                }
            }
        }
        
        [SerializeField] private int guid;
        [SerializeField] private BoolEvent enabledChanged;

        [NonSerialized] private bool isEnabled;

        #endregion

        public void AddOnEnabledChangedCallback(Action<bool> callback)
        {
            enabledChanged.AddListener(callback);
        }

        public void RemoveOnEnabledChangedCallback(Action<bool> callback)
        {
            enabledChanged.RemoveListener(callback);
        }
    }
}
