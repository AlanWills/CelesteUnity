﻿using Celeste.Events;
using UnityEngine;

namespace Celeste.Parameters
{
    [CreateAssetMenu(fileName = nameof(FloatValue), menuName = "Celeste/Parameters/Numeric/Float/Float Value")]
    public class FloatValue : ParameterValue<float, FloatValueChangedEvent>
    {
        #region Operators

        public static bool operator ==(FloatValue value, float f)
        {
            return value.Value == f;
        }

        public static FloatValue operator +(FloatValue value, float f)
        {
            value.Value += f;
            return value;
        }

        public static FloatValue operator -(FloatValue value, float f)
        {
            value.Value -= f;
            return value;
        }

        public static bool operator !=(FloatValue value, float f)
        {
            return value.Value != f;
        }

        #endregion

        #region

        public override bool Equals(object obj)
        {
            return obj is FloatValue value &&
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