using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(StringValue), menuName = "Celeste/Parameters/String/String Value")]
    public class StringValue : ParameterValue<string>
    {
        #region Operators

        public static bool operator ==(StringValue value, string s)
        {
            return value is null ? s == null : string.CompareOrdinal(value.Value, s) == 0;
        }

        public static StringValue operator +(StringValue value, string s)
        {
            value.Value += s;
            return value;
        }

        public static bool operator !=(StringValue value, string s)
        {
            return !(value == s);
        }

        #endregion

        #region Equals / HashCode

        public override bool Equals(object obj)
        {
            return obj is StringValue value &&
                   base.Equals(obj) &&
                   Value == value.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = -159790080;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        #endregion
    }
}
