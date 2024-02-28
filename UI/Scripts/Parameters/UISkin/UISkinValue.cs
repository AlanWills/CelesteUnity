using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI.Skin;
using UnityEngine;

namespace Celeste.UI.Parameters
{
    [CreateAssetMenu(fileName = nameof(UISkinValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "UI/UI Skin Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class UISkinValue : ParameterValue<UISkin, UISkinValueChangedEvent>
    {
    }
}