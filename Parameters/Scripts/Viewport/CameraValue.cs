using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(CameraValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Viewport/Camera Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class CameraValue : ParameterValue<Camera, CameraValueChangedEvent>
    {
    }
}
