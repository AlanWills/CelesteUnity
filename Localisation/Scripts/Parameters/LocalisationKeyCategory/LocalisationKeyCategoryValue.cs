using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Localisation/Localisation Key Category Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyCategoryValue : ParameterValue<LocalisationKeyCategory, LocalisationKeyCategoryValueChangedEvent>
    {
    }
}
