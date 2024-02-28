using System.Collections;
using UnityEngine;

namespace Celeste.LocationServices
{
    [CreateAssetMenu(fileName = nameof(LocationServicesRecord), menuName = CelesteMenuItemConstants.LOCATIONSERVICES_MENU_ITEM + "Location Services Record", order = CelesteMenuItemConstants.LOCATIONSERVICES_MENU_ITEM_PRIORITY)]
    public class LocationServicesRecord : ScriptableObject
    {
        #region Properties and Fields

        public int UpdateCount { get; private set; }
        public int PedometerSteps { get; private set; }
        public double PedometerDistance { get; private set; }

        #endregion

        public void OnStep(int stepsIncrement, double distanceIncrement)
        {
            ++UpdateCount;
            PedometerSteps += stepsIncrement;
            PedometerDistance += distanceIncrement;
        }
    }
}