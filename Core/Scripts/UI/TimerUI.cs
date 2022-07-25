using Celeste.Tools;
using TMPro;
using UnityEngine;

namespace Celeste.Core.UI
{
    [AddComponentMenu("Celeste/Core/UI/Timer UI")]
    public class TimerUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private Timer timer;
        [SerializeField] private TextMeshProUGUI timerText;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGet(ref timerText);
        }

        private void OnEnable()
        {
            timer.AddTimerUpdateCallback(OnTimerUpdate);

            SetText(timer.RemainingTime);
        }

        private void OnDisable()
        {
            timer.RemoveTimerUpdateCallback(OnTimerUpdate);
        }

        #endregion

        private void SetText(float remainingTime)
        {
            timerText.text = TimeUtility.FormatTimeString(Mathf.CeilToInt(remainingTime));
        }

        #region Callbacks

        private void OnTimerUpdate(float remainingTime)
        {
            SetText(remainingTime);
        }

        #endregion
    }
}
