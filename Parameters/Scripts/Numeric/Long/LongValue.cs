﻿using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(LongValue), menuName = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM + "Numeric/Long Value", order = CelesteMenuItemConstants.PARAMETERS_MENU_ITEM_PRIORITY)]
    public class LongValue : ParameterValue<long, LongValueChangedEvent>
    {
        #region Operators

        public static bool operator ==(LongValue value, long l)
        {
            return value.Value == l;
        }

        public static LongValue operator +(LongValue value, long l)
        {
            value.Value += l;
            return value;
        }

        public static LongValue operator -(LongValue value, long l)
        {
            value.Value -= l;
            return value;
        }

        public static bool operator !=(LongValue value, long l)
        {
            return value.Value != l;
        }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            return obj is LongValue value &&
                   base.Equals(obj) &&
                   Value == value.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = -159790080;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}
