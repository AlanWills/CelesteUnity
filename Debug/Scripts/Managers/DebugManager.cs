using System;
using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Debug.Managers
{
    public class DebugManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private BoolValue isDeveloperConsoleEnabled;

        private const string kIsDeveloperConsoleEnabledKey = "Celeste_Debug_IsDeveloperConsoleEnabled";
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            isDeveloperConsoleEnabled.Value = PlayerPrefs.GetInt(kIsDeveloperConsoleEnabledKey) > 0;
            isDeveloperConsoleEnabled.AddValueChangedCallback(OnIsDeveloperConsoleEnabledChanged);
            UnityEngine.Debug.developerConsoleVisible = isDeveloperConsoleEnabled.Value;
        }

        private void OnDisable()
        {
            isDeveloperConsoleEnabled.RemoveValueChangedCallback(OnIsDeveloperConsoleEnabledChanged);
        }

        #endregion
        
        #region Callbacks

        private void OnIsDeveloperConsoleEnabledChanged(ValueChangedArgs<bool> args)
        {
            UnityEngine.Debug.developerConsoleVisible = args.newValue;
            PlayerPrefs.SetInt(kIsDeveloperConsoleEnabledKey, args.newValue ? 1 : 0);
            PlayerPrefs.Save();
        }
        
        #endregion
    }
}