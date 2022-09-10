using UnityEngine;
using Celeste.Events;

namespace Celeste.Localisation
{
	public class LanguageValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<Language>, LanguageValueChangedEvent>
	{
	}
}
