using Celeste.Events;
using UnityEditor;

namespace CelesteEditor.Events
{
    [CustomPropertyDrawer(typeof(GuaranteedFloatEvent))]
    public class GuaranteedFloatEventPropertyDrawer : GuaranteedParameterisedEventPropertyDrawer
    {
    }
}
