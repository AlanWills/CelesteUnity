using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters.Rendering
{
    [CreateAssetMenu(fileName = "SpriteReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Rendering/Sprite Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class SpriteReference : ParameterReference<Sprite, SpriteValue, SpriteReference>
    {
    }
}
