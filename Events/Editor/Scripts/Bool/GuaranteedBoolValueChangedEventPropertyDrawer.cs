using Celeste.Events;
using UnityEditor;

namespace CelesteEditor.Events
{
    [CustomPropertyDrawer(typeof(GuaranteedBoolValueChangedEvent))]
    public class GuaranteedBoolValueChangedEventPropertyDrawer : GuaranteedParameterisedEventPropertyDrawer
    {
    }
}
