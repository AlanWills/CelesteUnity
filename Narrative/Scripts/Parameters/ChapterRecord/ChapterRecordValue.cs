using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Narrative.Parameters
{
    [CreateAssetMenu(fileName = nameof(ChapterRecordValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Narrative/Chapter Record Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class ChapterRecordValue : ParameterValue<ChapterRecord, ChapterRecordValueChangedEvent>
    {
    }
}
