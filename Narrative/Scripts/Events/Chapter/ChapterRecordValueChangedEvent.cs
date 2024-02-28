using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Narrative;

namespace Celeste.Events 
{
	[Serializable]
	public class ChapterRecordValueChangedUnityEvent : ValueChangedUnityEvent<ChapterRecord> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(ChapterRecordValueChangedEvent), menuName = CelesteMenuItemConstants.EVENTS_MENU_ITEM + "Narrative/Chapter Record Value Changed Event", order = CelesteMenuItemConstants.EVENTS_MENU_ITEM_PRIORITY)]
	public class ChapterRecordValueChangedEvent : ParameterisedValueChangedEvent<ChapterRecord>
	{
	}
}
