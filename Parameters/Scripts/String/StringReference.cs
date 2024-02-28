﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Parameters
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(StringReference), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "String/String Reference", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class StringReference : ParameterReference<string, StringValue, StringReference>
    {
        #region Operators

        public static bool operator ==(StringReference reference, string s)
        {
            return reference == (object)null ? s == null : string.CompareOrdinal(reference.Value, s) == 0;
        }

        public static StringReference operator +(StringReference reference, string s)
        {
            reference.Value += s;
            return reference;
        }

        public static bool operator !=(StringReference reference, string s)
        {
            return !(reference == s);
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
