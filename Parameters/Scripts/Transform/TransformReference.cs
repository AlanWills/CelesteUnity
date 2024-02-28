using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "TransformReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Transform/Transform Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class TransformReference : ParameterReference<Transform, TransformValue, TransformReference>
    {
    }
}
