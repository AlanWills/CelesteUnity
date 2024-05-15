using System;
using TMPro;
using UnityEngine;

namespace Celeste.Log
{
    [AddComponentMenu("Celeste/Log/Hud Message")]
    public class HudMessage : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float lifeTime = 3;

        private float timeAlive = 0;
        private Action onLifetimeOver;

        #endregion

        #region Unity Methods

        private void OnDisable()
        {
            text.text = "";
            timeAlive = 0;
            onLifetimeOver = default;
        }

        private void Update()
        {
            timeAlive += Time.unscaledDeltaTime;

            if (timeAlive > lifeTime)
            {
                onLifetimeOver();
            }
        }

        #endregion

        public void SetUp(string message, Color colour, Action onLifetimeOver)
        {
            text.text = message;
            text.color = colour;
            this.onLifetimeOver = onLifetimeOver;
        }
    }
}
