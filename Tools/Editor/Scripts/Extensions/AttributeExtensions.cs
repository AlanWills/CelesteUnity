using System;
using System.ComponentModel;
using System.Reflection;

namespace CelesteEditor.Tools
{
    public static class AttributeExtensions
    {
        public static string GetDisplayName(this Type t)
        {
            DisplayNameAttribute displayName = t.GetCustomAttribute<DisplayNameAttribute>(false);
            return displayName != null ? displayName.DisplayName : t.Name;
        }
    }
}
