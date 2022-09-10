using UnityEngine;
using Celeste.Events;
using Celeste.Narrative;

namespace Celeste.Events
{
	public class ChapterRecordValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<ChapterRecord>, ChapterRecordValueChangedEvent>
	{
	}
}
