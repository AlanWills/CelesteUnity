using Celeste.Events;
using Celeste.Parameters;
using Celeste.UI.Nodes;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.UI
{
    [CreateAssetMenu(fileName = nameof(NameEntryPopupArgs), menuName = "Celeste/Narrative/UI/Name Entry Popup Args")]
    public class NameEntryPopupArgs : ShowPopupArgs, IPopupArgs
    {
        public StringValue nameToEdit;
        public List<StringValue> reservedNames = new List<StringValue>();
    }
}