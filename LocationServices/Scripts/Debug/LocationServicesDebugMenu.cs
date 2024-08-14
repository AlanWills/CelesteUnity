using Celeste.Debug.Menus;
using UnityEngine;
#if USE_NEW_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Celeste.LocationServices
{
    [CreateAssetMenu(fileName = nameof(LocationServicesDebugMenu), menuName = CelesteMenuItemConstants.LOCATIONSERVICES_MENU_ITEM + "Location Services Debug Menu", order = CelesteMenuItemConstants.LOCATIONSERVICES_MENU_ITEM_PRIORITY)]
    public class LocationServicesDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private LocationServicesRecord locationServicesRecord;

        #endregion

        protected override void OnDrawMenu()
        {
            if (locationServicesRecord.UpdateCount == 0)
            {
                GUILayout.Label($"Pedometer Still Initializing");
            }
            else
            {
                GUILayout.Label($"Pedometer Steps: {locationServicesRecord.PedometerSteps}");
                GUILayout.Label($"Pedometer Distance: {locationServicesRecord.PedometerDistance}");
                GUILayout.Label($"Pedometer Update: {locationServicesRecord.UpdateCount}");
            }

#if USE_NEW_INPUT_SYSTEM
            if (StepCounter.current != null)
            {
                StepCounter currentStepCounter = StepCounter.current;
                GUILayout.Label($"Step Counter Steps: {currentStepCounter.stepCounter.ReadValue()}");
                GUILayout.Label($"Step Counter Updated: {currentStepCounter.lastUpdateTime}");
                GUILayout.Label($"Step Counter Background: {currentStepCounter.canRunInBackground}");
            }
            else
            {
                GUILayout.Label("Current Step Counter null");
            }
#endif
        }
    }
}
