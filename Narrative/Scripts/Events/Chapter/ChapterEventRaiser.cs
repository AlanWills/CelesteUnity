using Celeste.Narrative;
using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Chapter/Chapter Event Raiser")]
    public class ChapterEventRaiser : ParameterisedEventRaiser<Chapter, ChapterEvent>
    {
    }
}
