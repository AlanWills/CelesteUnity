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
    [CustomEditor(typeof(StringEvent))]
    public class StringEventEditor : ParameterisedEventEditor<string, StringEvent>
    {
        protected override string DrawArgument(string argument)
        {
            return EditorGUILayout.TextField(argument);
        }
    }
}
