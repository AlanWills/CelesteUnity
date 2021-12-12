using PedometerU;
using PedometerU.Platforms;
using System.Collections;
using UnityEngine;

namespace Celeste.LocationServices.Platforms
{
    public class PedometerDisabled : IPedometer
    {
        bool IPedometer.IsSupported
        {
            get { return false; }
        }

        event StepCallback IPedometer.OnStep
        {
            add { }
            remove { }
        }
    }
}