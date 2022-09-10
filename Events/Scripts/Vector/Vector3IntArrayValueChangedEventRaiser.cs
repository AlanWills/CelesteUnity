using UnityEngine;
using Celeste.Events;
using System.Collections.Generic;

namespace Celeste.Events
{
	public class Vector3IntArrayValueChangedEventRaiser : ParameterisedEventRaiser<ValueChangedArgs<List<Vector3Int>>, Vector3IntArrayValueChangedEvent>
	{
	}
}
