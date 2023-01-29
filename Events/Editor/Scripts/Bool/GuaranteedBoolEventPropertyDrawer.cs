using Celeste.Events;
using UnityEditor;

namespace CelesteEditor.Events
{
    [CustomPropertyDrawer(typeof(GuaranteedBoolEvent))]
    public class GuaranteedBoolEventPropertyDrawer : GuaranteedParameterisedEventPropertyDrawer
    {
    }
}
