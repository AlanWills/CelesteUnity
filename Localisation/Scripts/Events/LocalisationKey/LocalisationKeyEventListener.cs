using Celeste.Localisation;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Localisation/Localisation Key Listener")]
    public class LocalisationKeyEventListener : ParameterisedEventListener<LocalisationKey, LocalisationKeyEvent, LocalisationKeyUnityEvent>
    {
    }
}
