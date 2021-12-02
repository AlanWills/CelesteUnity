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

    [Serializable]
    public class ValueRecord
    {
        #region Properties and Fields

        public string Name { get { return value.name; } }
        public ValueType Type { get; }
        public object Value
        { 
            get 
            {
                switch (Type)
                {
                    case ValueType.String:
                        return (value as ParameterValue<string>).Value;
                    
                    case ValueType.Bool:
                        return (value as ParameterValue<bool>).Value;

                    default:
                        UnityEngine.Debug.LogAssertion($"Unhandled {nameof(ValueType)} {Type} in {nameof(ValueRecord)} {Name}.");
                        return null;
                }
            }
            set
            {
                switch (Type)
                {
                    case ValueType.String:
                        (this.value as ParameterValue<string>).Value = value as string;
                        break;

                    case ValueType.Bool:
                        (this.value as ParameterValue<bool>).Value = (bool)value;
                        break;

                    default:
                        UnityEngine.Debug.LogAssertion($"Unhandled {nameof(ValueType)} {Type} in {nameof(ValueRecord)} {Name}.");
                        break;
                }
            }
        }

        private ScriptableObject value;

        #endregion

        public ValueRecord(StringValue value)
        {
            this.value = value;
            Type = ValueType.String;
        }

        public ValueRecord(BoolValue value)
        {
            this.value = value;
            Type = ValueType.Bool;
        }
    }
}