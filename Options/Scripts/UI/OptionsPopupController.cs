using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Celeste.Options.UI
{
    [AddComponentMenu("Celeste/Options/UI/Options Popup Controller")]
    public class OptionsPopupController : MonoBehaviour, IPopupController
    {
        #region Properties and Fields

        [Header("UI Elements")]
        [SerializeField] private Toggle batterySaverToggle;

        [Header("Options")]
        [SerializeField] private IntValue defaultFrameRate;

        private const int BATTERY_SAVER_FRAME_RATE = 30;
        private const int NON_BATTERY_SAVER_FRAME_RATE = 60;

        #endregion

        public void OnShow(IPopupArgs args)
        {
            batterySaverToggle.isOn = defaultFrameRate == BATTERY_SAVER_FRAME_RATE;
        }

        public void OnHide()
        {
        }

        public void OnConfirmPressed()
        {
        }

        public void OnClosePressed()
        {
        }

        #region Callbacks

        public void OnBatterySaverChanged(bool isBatterySaverEnabled)
        {
            defaultFrameRate.Value = isBatterySaverEnabled ? BATTERY_SAVER_FRAME_RATE : NON_BATTERY_SAVER_FRAME_RATE;
        }

        #endregion
    }
}
