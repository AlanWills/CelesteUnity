using Celeste.Events;
using Celeste.Parameters;
using UnityEngine;
using UnityEngine.Events;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(FloatOption), menuName = "Celeste/Options/Float/Float Option")]
    public class FloatOption : Option<float, FloatValue>
    {
    }
}
