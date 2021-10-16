using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringReference", menuName = "Celeste/Parameters/String/String Reference")]
    public class StringReference : ParameterReference<string, StringValue, StringReference>
    {
        #region Operators

        public static bool operator ==(StringReference reference, string s)
        {
            return reference.Value == s;
        }

        public static StringReference operator +(StringReference reference, string s)
        {
            reference.Value += s;
            return reference;
        }

        public static bool operator !=(StringReference reference, string s)
        {
            return reference.Value != s;
        }

        #endregion

        #region Equals & HashCode

        public override bool Equals(object obj)
        {
            return obj is StringReference reference &&
                   base.Equals(obj) &&
                   Value == reference.Value;
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
