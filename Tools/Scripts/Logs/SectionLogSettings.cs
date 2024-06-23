using System;
using UnityEngine;

namespace Celeste.Log
{
    [CreateAssetMenu(fileName = nameof(SectionLogSettings), menuName = CelesteMenuItemConstants.LOG_MENU_ITEM + "Section Log Settings", order = CelesteMenuItemConstants.LOG_MENU_ITEM_PRIORITY)]
    public class SectionLogSettings : ScriptableObject
    {
        [Flags]
        private enum LogTypeBitField
        {
            Log = 1 << 0,
            Warning = 1 << 1,
            Error = 1 << 2,
            Assertion = 1 << 3,
            Exception = 1 << 4
        }

        #region Properties and Fields

        public UnityEngine.Object LogContext { get; set; }
        public string SectionName => sectionName;
        private string ColourPreamble => $"<color=#{sectionColour.r:X2}{sectionColour.g:X2}{sectionColour.b:X2}>[{sectionName}]</color>";

        [SerializeField] private string sectionName;
        [SerializeField] private Color32 sectionColour;
        [SerializeField] private LogTypeBitField typesToLogToHudAutomatically = LogTypeBitField.Error | LogTypeBitField.Assertion | LogTypeBitField.Exception;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(sectionName))
            {
                sectionName = name.Replace(nameof(SectionLogSettings), string.Empty);
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        #endregion

        public bool ShouldLogToHud(LogType logType)
        {
            LogTypeBitField logTypeBitField = ToLogTypeBitField(logType);
            return typesToLogToHudAutomatically.HasFlag(logTypeBitField);
        }

        public string FormatLogMessage(string format, params object[] args)
        {
            if (args.Length == 1)
            {
                // Small optimisation for the case of raw logs with no formatting - Unity sends these through as a single arg with the format '{0}'
                return $"{ColourPreamble} {args[0]}";
            }

            return $"{ColourPreamble} {string.Format(format, args)}";
        }

        public string FormatException(Exception e)
        {
            return $"{ColourPreamble} {e.Message}";
        }

        public SectionLogSettings WithContext(UnityEngine.Object logContext)
        {
            // A bit of a hack around the fact we're passing this as the context to a log, but may still want to pass an actual object for the context to Unity logs (and others)
            LogContext = logContext;
            return this;
        }

        private static LogTypeBitField ToLogTypeBitField(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    return LogTypeBitField.Log;
                case LogType.Warning:
                    return LogTypeBitField.Warning;
                case LogType.Error:
                    return LogTypeBitField.Error;
                case LogType.Assert:
                    return LogTypeBitField.Assertion;
                case LogType.Exception:
                    return LogTypeBitField.Exception;
                default:
                    return LogTypeBitField.Log;
            }
        }
    }
}