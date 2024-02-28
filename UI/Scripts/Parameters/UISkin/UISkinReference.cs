using Celeste.Parameters;
using Celeste.UI.Skin;
using UnityEngine;

namespace Celeste.UI.Parameters
{
    [CreateAssetMenu(fileName = nameof(UISkinReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "UI/UI Skin Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class UISkinReference : ParameterReference<UISkin, UISkinValue, UISkinReference>
    {
    }
}
