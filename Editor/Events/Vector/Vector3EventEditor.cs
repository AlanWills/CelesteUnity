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
    [CustomEditor(typeof(Vector3Event))]
    public class Vector3EventEditor : ParameterisedEventEditor<Vector3, Vector3Event>
    {
        protected override Vector3 DrawArgument(Vector3 argument)
        {
            return EditorGUILayout.Vector3Field(GUIContent.none, argument);
        }
    }
}
