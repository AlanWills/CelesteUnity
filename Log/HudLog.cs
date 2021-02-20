using Celeste.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Hud Log")]
    public class HudLog : Singleton<HudLog>
    {
        private class HudMessage
        {
            public Text message;
            public float timeAlive;
        }

        #region Properties and Fields

        public Transform textParent;
        public GameObject hudMessagePrefab;
        public Color infoColour = Color.white;
        public Color warningColour = Color.yellow;
        public Color errorColour = Color.red;
        public float messageLifetime = 3;

        [SerializeField]
        private int maxMessages = 20;

        private List<HudMessage> hudMessages = new List<HudMessage>();
        private Stack<Text> cachedMessageInstances = new Stack<Text>();

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

        public static void LogInfoFormat(string format, params object[] args)
        {
            LogInfo(string.Format(format, args));
        }

        public static void LogWarning(string message)
        {
            if (Instance != null)
            {
                Instance.Log(message, Instance.warningColour);
                UnityEngine.Debug.LogWarning(message);
            }
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            LogWarning(string.Format(format, args));
        }

        public static void LogError(string message)
        {
            if (Instance != null)
            {
                Instance.Log(message, Instance.errorColour);
                UnityEngine.Debug.LogError(message);
            }
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            LogError(string.Format(format, args));
        }

        private void Log(string message, Color colour)
        {
            if (cachedMessageInstances.Count > 0)
            {
                Text messageText = cachedMessageInstances.Pop();
                messageText.text = message;
                messageText.color = colour;
                messageText.gameObject.SetActive(true);

                hudMessages.Add(new HudMessage() { message = messageText, timeAlive = 0 });
            }
            else
            {
                UnityEngine.Debug.LogWarningFormat("Hud Message limit reached.  Message: {0}", message);
            }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < maxMessages; ++i)
            {
                GameObject gameObject = GameObject.Instantiate(hudMessagePrefab, textParent);
                Text messageText = gameObject.GetComponent<Text>();
                gameObject.SetActive(false);

                cachedMessageInstances.Push(messageText);
            }

            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int i = hudMessages.Count; i > 0; --i)
            {
                HudMessage hudMessage = hudMessages[i - 1];
                hudMessage.timeAlive += deltaTime;

                if (hudMessage.timeAlive > messageLifetime)
                {
                    Text messageText = hudMessage.message;
                    messageText.text = "";
                    messageText.gameObject.SetActive(false);

                    hudMessages.RemoveAt(i - 1);
                    cachedMessageInstances.Push(messageText);
                }
            }
        }

        #endregion

        #region Callbacks

        private void Application_logMessageReceived(string logString, string stackTrace, LogType type)
        {
            if (type == LogType.Exception || type == LogType.Assert)
            {
                LogError(logString);
            }
        }

        #endregion
    }
}
