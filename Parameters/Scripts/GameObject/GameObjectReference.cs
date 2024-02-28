using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = "GameObjectReference", menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Game Object/GameObject Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class GameObjectReference : ParameterReference<GameObject, GameObjectValue, GameObjectReference>
    {
    }
}
