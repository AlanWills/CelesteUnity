using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringValue", menuName = "Celeste/Parameters/String/String Value")]
    public class StringValue : ParameterValue<string>
    {
        #region Operators

        public static bool operator ==(StringValue value, string s)
        {
            return value.Value == s;
        }

        public static StringValue operator +(StringValue value, string s)
        {
            value.Value += s;
            return value;
        }

        public static bool operator !=(StringValue value, string s)
        {
            return value.Value != s;
        }

        #endregion

        #region

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
