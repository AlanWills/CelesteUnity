using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers.Parameters
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferencePropertyDrawer : ParameterReferencePropertyDrawer
    {
    }
}
