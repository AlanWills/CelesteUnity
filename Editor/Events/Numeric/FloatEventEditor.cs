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
    [CustomEditor(typeof(FloatEvent))]
    public class FloatEventEditor : ParameterisedEventEditor<float, FloatEvent>
    {
        protected override float DrawArgument(float argument)
        {
            return EditorGUILayout.FloatField(argument);
        }
    }
}
