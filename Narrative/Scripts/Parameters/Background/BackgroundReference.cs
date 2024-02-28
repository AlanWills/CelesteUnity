using Celeste.Narrative.Backgrounds;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(BackgroundReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Narrative/Background Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class BackgroundReference : ParameterReference<Background, BackgroundValue, BackgroundReference>
    {
    }
}
