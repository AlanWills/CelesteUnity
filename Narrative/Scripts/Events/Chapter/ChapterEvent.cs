using Celeste.Narrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class ChapterUnityEvent : UnityEvent<Chapter> { }

    [Serializable]
    [CreateAssetMenu(fileName = "ChapterEvent", menuName = "Celeste/Events/Chapter Event")]
    public class ChapterEvent : ParameterisedEvent<Chapter>
    {
    }
}
