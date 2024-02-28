using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(FloatOption), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Float/Float Option", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class FloatOption : Option<float, FloatValue>
    {
    }
}
