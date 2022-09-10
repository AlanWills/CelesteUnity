using UnityEngine;
using Celeste.Events;
using Celeste.Narrative;

namespace Celeste.Events
{
	public class ChapterRecordValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<ChapterRecord>, ChapterRecordValueChangedEvent, ChapterRecordValueChangedUnityEvent>
	{
	}
}
