using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(BoolOption), menuName = "Celeste/Options/Bool/Bool Option")]
    public class BoolOption : Option<bool, BoolValue>
    {
    }
}
