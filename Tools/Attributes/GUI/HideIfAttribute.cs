using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }

        public HideIfAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
