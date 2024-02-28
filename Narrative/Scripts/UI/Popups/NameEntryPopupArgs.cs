using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [CreateAssetMenu(fileName = nameof(NameEntryPopupArgs), menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "UI/Name Entry Popup Args", order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NameEntryPopupArgs : ShowPopupArgs, IPopupArgs
    {
        public StringValue nameToEdit;
        public List<StringValue> reservedNames = new List<StringValue>();
    }
}