using Celeste.Parameters;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(StringOption), menuName = "Celeste/Options/String/String Option")]
    public class StringOption : Option<string, StringValue>
    {
    }
}
