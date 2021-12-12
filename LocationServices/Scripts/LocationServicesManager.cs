using PedometerU;
using System.Collections;
using UnityEngine;

namespace Celeste.LocationServices
{
    [AddComponentMenu("Celeste/Location Services/Location Services Manager")]
    public class LocationServicesManager : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private LocationServicesRecord locationServicesRecord;

        private Pedometer pedometer;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            pedometer = new Pedometer(locationServicesRecord.OnStep);
        }

        private void OnDestroy()
        {
            pedometer.Dispose();
        }

        #endregion
    }
}