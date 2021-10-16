using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    [CustomEditor(typeof(GameObjectClickEvent))]
    public class GameObjectClickEventEditor : ParameterisedEventEditor<GameObjectClickEventArgs, GameObjectClickEvent>
    {
        protected override GameObjectClickEventArgs DrawArgument(GameObjectClickEventArgs argument)
        {
            argument.clickWorldPosition = EditorGUILayout.Vector3Field(GUIContent.none, argument.clickWorldPosition);
            argument.gameObject = EditorGUILayout.ObjectField(argument.gameObject, typeof(GameObject), true) as GameObject;

            return argument;
        }
    }
}
