using Celeste.Memory;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Hud Log")]
    public class HudLog : SceneSingleton<HudLog>
    {
        #region Properties and Fields

        [SerializeField] private HudLogDebugMenu hudLogDebugMenu;
        [SerializeField] private GameObjectAllocator hudMessages;
        [SerializeField] private Color infoColour = Color.white;
        [SerializeField] private Color warningColour = Color.yellow;
        [SerializeField] private Color errorColour = Color.red;

        #endregion

        #region Logging Methods

        public static void LogInfo(string message)
        {
            if (Instance != null)
            {
                Instance.Log(message, Instance.infoColour);
                UnityEngine.Debug.Log(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (Instance != null)
            {
                Instance.Log(message, Instance.warningColour);
                UnityEngine.Debug.LogWarning(message);
            }
        }

        public static void LogError(string message)
        {
            if (Instance != null)
            {
                Instance.Log(message, Instance.errorColour);
                UnityEngine.Debug.LogError(message);
            }
        }

        private void Log(string message, Color colour)
        {
            if (hudMessages.CanAllocate(1))
            {
                GameObject messageGameObject = hudMessages.Allocate();
                HudMessage hudMessage = messageGameObject.GetComponent<HudMessage>();
                hudMessage.SetUp(message, colour, () => { hudMessages.Deallocate(messageGameObject); });
                messageGameObject.SetActive(true);
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat("Hud Message limit reached.  Message: {0}", message);
            }

            if (hudLogDebugMenu != null)
            {
                hudLogDebugMenu.AddLogEntry(message);
            }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        #endregion

        #region Callbacks

        private void Application_logMessageReceived(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Assert)
            {
                if (Instance != null)
                {
                    Instance.Log(logString, Instance.errorColour);
                }
            }
        }

        #endregion
    }
}
