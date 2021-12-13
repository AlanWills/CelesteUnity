using Celeste.Parameters;
using System;
using UnityEngine;

namespace Celeste.Narrative
{
    public enum ValueType
    {
        String,
        Bool
    }

    public class ValueRecord
    {
        #region Properties and Fields

        public string Name { get; }
        public ValueType Type { get; }
        public object Value { get; set; }

        #endregion

        public ValueRecord(StringValue value)
        {
            Name = value.name;
            Value = value.Value;
            Type = ValueType.String;
        }

        public ValueRecord(BoolValue value)
        {
            Name = value.name;
            Value = value.Value;
            Type = ValueType.Bool;
        }

        public void ApplyTo(StringValue value)
        {
            if (CheckType(ValueType.String))
            {
                value.Value = Value as string;
            }
        }

        public void ApplyTo(BoolValue value)
        {
            if (CheckType(ValueType.Bool))
            {
                value.Value = (bool)Value;
            }
        }

        private bool CheckType(ValueType valueType)
        {
            UnityEngine.Debug.Assert(Type == valueType, $"Value Record '{Name}' is '{Type}' not '{valueType}'.");
            return Type == valueType;
        }
    }
}