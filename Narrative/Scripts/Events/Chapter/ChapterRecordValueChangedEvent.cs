using System;
using UnityEngine;
using Celeste.Events;
using Celeste.Narrative;

namespace Celeste.Events 
{
	[Serializable]
	public class ChapterRecordValueChangedUnityEvent : ValueChangedUnityEvent<ChapterRecord> { }

	[Serializable]
	[CreateAssetMenu(fileName = nameof(ChapterRecordValueChangedEvent), menuName = "Celeste/Events/Narrative/Chapter Record Value Changed Event")]
	public class ChapterRecordValueChangedEvent : ParameterisedValueChangedEvent<ChapterRecord>
	{
	}
}
