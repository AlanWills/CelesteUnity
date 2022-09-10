using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Application
{
    [AddComponentMenu("Celeste/Application/Frame Rate Controller")]
    public class ApplicationFrameRateController : MonoBehaviour
    {
        #region Properties and Fields

        private int FrameRate
        {
            set 
            { 
                UnityEngine.Application.targetFrameRate = value; 
            }
        }

        [SerializeField] private IntValue defaultFrameRate;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            FrameRate = defaultFrameRate.Value;

            defaultFrameRate.AddValueChangedCallback(OnDefaultFrameRateChanged);
        }

        private void OnDestroy()
        {
            defaultFrameRate.AddValueChangedCallback(OnDefaultFrameRateChanged);
        }

        #endregion

        #region Callbacks

        private void OnDefaultFrameRateChanged(ValueChangedArgs<int> args)
        {
            FrameRate = args.newValue;
        }

        #endregion
    }
}
