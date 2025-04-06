using Celeste.Events;
using Celeste.UI.Nodes;
using CelesteEditor.Events;
using UnityEditor;

namespace CelesteEditor.UI.Events
{
    [CustomEditor(typeof(ShowPopupEvent))]
    public class ShowPopupEventEditor : ParameterisedEventEditor<IPopupArgs, ShowPopupEvent>
    {
        protected override IPopupArgs DrawArgument(IPopupArgs argument)
        {
            EditorGUILayout.LabelField("No Popup Args");
            return new NoPopupArgs();
        }
    }
}
