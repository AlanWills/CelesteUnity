using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfEnumAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }
        public int Value { get; private set; }

        public HideIfEnumAttribute(string propertyName, int value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}
