using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(IntOption), menuName = "Celeste/Options/Int/Int Option")]
    public class IntOption : Option<int, IntValue>
    {
    }
}
