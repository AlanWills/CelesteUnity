using Celeste.Events;
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
        [SerializeField] private Events.Event onTimerEnd;

        #endregion

        public void StartTimer(int timerTime)
        {
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
            paused.Value = true;
            onTimerEnd.Invoke();
        }

        #region Callbacks

        public void AddTimerUpdateCallback(UnityAction<ValueChangedArgs<float>> onTimerUpdateCallback)
        {
            remainingTime.AddValueChangedCallback(onTimerUpdateCallback);
        }

        public void RemoveTimerUpdateCallback(UnityAction<ValueChangedArgs<float>> onTimerUpdateCallback)
        {
            remainingTime.RemoveValueChangedCallback(onTimerUpdateCallback);
        }

        #endregion
    }
}
