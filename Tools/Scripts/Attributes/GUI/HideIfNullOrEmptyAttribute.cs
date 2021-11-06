using UnityEngine;

namespace Celeste.Tools.Attributes.GUI
{
    public class HideIfNullOrEmptyAttribute : PropertyAttribute
    {
        public string PropertyName { get; private set; }

        public HideIfNullOrEmptyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
