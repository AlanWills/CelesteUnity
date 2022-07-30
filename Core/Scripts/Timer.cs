using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Core
{
    [CreateAssetMenu(fileName = nameof(Timer), menuName = "Celeste/Time/Timer")]
    public class Timer : ScriptableObject
    {
        #region Properties and Fields

        public float RemainingTime => remainingTime.Value;
        public bool IsPaused => paused.Value;

        [SerializeField] private FloatValue remainingTime;
        [SerializeField] private BoolValue paused;
        [SerializeField] private Events.FloatEvent onTimerUpdate;
        [SerializeField] private Events.Event onTimerEnd;

        #endregion

        public void StartTimer(int timerTime)
        {
            remainingTime.AddValueChangedCallback(OnRemainingTimeChanged);
            remainingTime.Value = timerTime;
            paused.Value = false;
        }

        public void UpdateTimer(float elapsedTime)
        {
            if (!paused.Value)
            {
                remainingTime.Value -= elapsedTime;

                if (RemainingTime <= 0)
                {
                    StopTimer();
                }
            }
        }

        public void StopTimer()
        {
            remainingTime.RemoveValueChangedCallback(OnRemainingTimeChanged);
            paused.Value = true;
            onTimerEnd.Invoke();
        }

        #region Callbacks

        private void OnRemainingTimeChanged(ValueChangedArgs<float> valueChangedArgs)
        {
            onTimerUpdate.InvokeSilently(valueChangedArgs.newValue);
        }

        public void AddTimerUpdateCallback(Action<float> onTimerUpdate)
        {
            this.onTimerUpdate.AddListener(onTimerUpdate);
        }

        public void RemoveTimerUpdateCallback(Action<float> onTimerUpdate)
        {
            this.onTimerUpdate.RemoveListener(onTimerUpdate);
        }

        #endregion
    }
}
