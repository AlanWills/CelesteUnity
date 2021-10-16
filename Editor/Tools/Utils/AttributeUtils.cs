using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CelesteEditor.Tools
{
    public static class AttributeUtils
    {
        public static string GetDisplayName(this Type t)
        {
            DisplayNameAttribute displayName = t.GetCustomAttribute<DisplayNameAttribute>();
            return displayName != null ? displayName.DisplayName : t.Name;
        }
    }
}
