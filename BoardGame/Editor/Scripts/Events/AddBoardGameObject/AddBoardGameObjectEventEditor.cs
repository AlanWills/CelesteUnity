using Celeste.Events;
using CelesteEditor.Events;
using UnityEditor;

namespace CelesteEditor.BoardGame.Events
{
    [CustomEditor(typeof(AddBoardGameObjectEvent))]
    public class AddBoardGameObjectEventEditor : ParameterisedEventEditor<AddBoardGameObjectArgs, AddBoardGameObjectEvent>
    {
        protected override AddBoardGameObjectArgs DrawArgument(AddBoardGameObjectArgs argument)
        {
            CelesteEditorGUILayout.TightLabel("Guid", 5);
            argument.boardGameObjectGuid = EditorGUILayout.IntField(argument.boardGameObjectGuid);

            CelesteEditorGUILayout.TightLabel("Location", 5);
            argument.location = EditorGUILayout.TextField(argument.location);

            return argument;
        }
    }
}
