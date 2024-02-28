using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI.Nodes;
using System.Collections;
using UnityEngine;

namespace Celeste.UI.Popups
{
    [CreateAssetMenu(fileName = nameof(StringEntryPopupArgs), order = CelesteMenuItemConstants.UI_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.UI_MENU_ITEM + "Popups/String Entry Popup Args")]
    public class StringEntryPopupArgs : ShowPopupArgs, IPopupArgs
    {
        public StringValue stringValue;
    }
}