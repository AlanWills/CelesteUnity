using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters.Rendering
{
    [CreateAssetMenu(fileName = nameof(SpriteValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Rendering/Sprite Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class SpriteValue : ParameterValue<Sprite, SpriteValueChangedEvent>
    {
    }
}
