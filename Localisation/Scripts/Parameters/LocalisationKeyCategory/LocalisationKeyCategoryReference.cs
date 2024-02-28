using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LocalisationKeyCategoryReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Localisation/Localisation Key Category Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LocalisationKeyCategoryReference : ParameterReference<LocalisationKeyCategory, LocalisationKeyCategoryValue, LocalisationKeyCategoryReference>
    {
    }
}
