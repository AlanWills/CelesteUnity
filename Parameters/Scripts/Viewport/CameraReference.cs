using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "New CameraReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Viewport/Camera Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class CameraReference : ParameterReference<Camera, CameraValue, CameraReference>
    {
    }
}
