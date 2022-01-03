using Celeste.Localisation;
using UnityEngine;

namespace Celeste.Events
{
    [AddComponentMenu("Celeste/Events/Localisation/Language Event Listener")]
    public class LanguageEventListener : ParameterisedEventListener<Language, LanguageEvent, LanguageUnityEvent>
    {
    }
}
