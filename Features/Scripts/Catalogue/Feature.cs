using Celeste.Objects;
using Celeste.Parameters;
using System;
using UnityEngine;

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

        #endregion

        public void AddOnEnabledChangedCallback(Action<bool> callback)
        {
            isEnabled.AddOnValueChangedCallback(callback);
        }

        public void RemoveOnEnabledChangedCallback(Action<bool> callback)
        {
            isEnabled.RemoveOnValueChangedCallback(callback);
        }
    }
}
