using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfNullAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }

        public HideIfNullAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
