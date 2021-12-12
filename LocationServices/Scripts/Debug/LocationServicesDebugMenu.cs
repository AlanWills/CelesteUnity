using Celeste.Debug.Menus;
using PedometerU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Celeste.LocationServices
{
    [CreateAssetMenu(fileName = nameof(LocationServicesDebugMenu), menuName = "Celeste/Location Services/Location Services Debug Menu")]
    public class LocationServicesDebugMenu : DebugMenu
    {
        #region Properties and Fields

        private Pedometer pedometer;
        private int pedometerSteps = 0;
        private double pedometerDistance = 0;

        #endregion

        protected override void OnShowMenu()
        {
            base.OnShowMenu();

            if (pedometer == null)
            {
                pedometer = new Pedometer(OnStep);
            }
        }

        protected override void OnDrawMenu()
        {
            if (pedometer.updateCount > 0)
            {
                GUILayout.Label($"Pedometer Still Initializing");
            }
            else
            {
                GUILayout.Label($"Pedometer Steps: {pedometerSteps}");
                GUILayout.Label($"Pedometer Distance: {pedometerDistance}");
                GUILayout.Label($"Pedometer Update: {pedometer.updateCount}");
            }
            
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
        }

        private void OnStep(int steps, double distance)
        {
            pedometerSteps = steps;
            pedometerDistance = distance;
        }
    }
}
