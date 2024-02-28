using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Localisation.Parameters
{
    [CreateAssetMenu(fileName = nameof(LanguageReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Localisation/Language Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LanguageReference : ParameterReference<Language, LanguageValue, LanguageReference>
    {
    }
}
