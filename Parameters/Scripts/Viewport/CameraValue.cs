using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(CameraValue), menuName = "Celeste/Parameters/Viewport/Camera Value")]
    public class CameraValue : ParameterValue<Camera>
    {
        protected override ParameterisedEvent<Camera> OnValueChanged => null;
    }
}
