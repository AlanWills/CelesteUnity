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
