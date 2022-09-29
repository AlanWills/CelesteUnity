using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Application
{
    [AddComponentMenu("Celeste/Application/Battery Saver")]
    public class BatterySaver : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private IntValue batterySaverDisabledFrameRate;
        [SerializeField] private IntValue batterySaverEnabledFrameRate;
        [SerializeField] private IntValue defaultFrameRate;

        #endregion

        #region Callbacks

        public void OnBatterySaverOptionChanged(ValueChangedArgs<bool> valueChangedArgs)
        {
            defaultFrameRate.Value = valueChangedArgs.newValue ? batterySaverEnabledFrameRate.Value : batterySaverDisabledFrameRate.Value;
        }

        #endregion
    }
}
