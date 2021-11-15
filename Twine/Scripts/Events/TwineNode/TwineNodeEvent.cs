using Celeste.Twine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Events
{
    [Serializable]
    public class TwineNodeUnityEvent : UnityEvent<TwineNode> { }

    [Serializable]
    [CreateAssetMenu(fileName = nameof(TwineNodeEvent), menuName = "Celeste/Events/Twine/Twine Node Event")]
    public class TwineNodeEvent : ParameterisedEvent<TwineNode>
    {
    }
}
