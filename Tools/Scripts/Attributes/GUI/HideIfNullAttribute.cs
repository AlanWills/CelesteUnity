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
