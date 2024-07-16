using Celeste.Events;
using Celeste.Parameters;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Core
{
    [CreateAssetMenu(fileName = nameof(Stopwatch), menuName = CelesteMenuItemConstants.CORE_MENU_ITEM + "Stopwatch", order = CelesteMenuItemConstants.CORE_MENU_ITEM_PRIORITY)]
    public class Stopwatch : ScriptableObject
    {
        #region Properties and Fields

        public float ElapsedSeconds => elapsedSeconds.Value;
        public bool IsPaused => paused.Value;

        [SerializeField] private FloatValue elapsedSeconds;
        [SerializeField] private BoolValue paused;

        #endregion

        public void Restart()
        {
            elapsedSeconds.Value = 0;
            paused.Value = false;
        }

        public void UpdateTimer(float elapsedTime)
        {
            if (!paused.Value)
            {
                elapsedSeconds.Value += elapsedTime;
            }
        }

        public void Pause()
        {
            paused.Value = true;
        }

        #region Callbacks

        public void AddStopwatchUpdateCallback(UnityAction<ValueChangedArgs<float>> onStopwatchUpdateCallback)
        {
            elapsedSeconds.AddValueChangedCallback(onStopwatchUpdateCallback);
        }

        public void RemoveStopwatchUpdateCallback(UnityAction<ValueChangedArgs<float>> onStopwatchUpdateCallback)
        {
            elapsedSeconds.RemoveValueChangedCallback(onStopwatchUpdateCallback);
        }

        #endregion
    }
}
