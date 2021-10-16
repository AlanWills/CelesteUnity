using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Narrative.Events
{
    [AddComponentMenu("Celeste/Events/Chapter/Chapter Event Listener")]
    public class ChapterEventListener : ParameterisedEventListener<Chapter, ChapterEvent, ChapterUnityEvent>
    {
    }
}
