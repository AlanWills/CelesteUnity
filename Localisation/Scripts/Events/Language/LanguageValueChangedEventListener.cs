using UnityEngine;
using Celeste.Events;

namespace Celeste.Localisation
{
	public class LanguageValueChangedEventListener : ParameterisedEventListener<ValueChangedArgs<Language>, LanguageValueChangedEvent, LanguageValueChangedUnityEvent>
	{
	}
}
