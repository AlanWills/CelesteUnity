using Celeste.Events;
using Celeste.Narrative.Backgrounds;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(BackgroundValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Narrative/Background Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class BackgroundValue : ParameterValue<Background, BackgroundValueChangedEvent>
    {
    }
}
