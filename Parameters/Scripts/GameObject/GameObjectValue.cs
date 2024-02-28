using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(GameObjectValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Game Object/GameObject Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class GameObjectValue : ParameterValue<GameObject, GameObjectValueChangedEvent>
    {
    }
}
