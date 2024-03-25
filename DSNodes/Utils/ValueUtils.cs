using System;
using XNode;

namespace Celeste.DSNodes
{
    public static class ValueUtils
    {
        public static float GetNumericInputValue(this Node node, string fieldName, float defaultValue = 0)
        {
            NodePort port = node.GetPort(fieldName);
            if (port == null)
            {
                return defaultValue;
            }

            object obj = port.GetInputValue();
            if (obj != null && obj.IsNumber())
            {
                return Convert.ToSingle(obj);
            }

            return defaultValue;
        }

        private static bool IsNumber(this object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
