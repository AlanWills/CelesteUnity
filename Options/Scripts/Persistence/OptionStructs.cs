using Celeste.Parameters;
using System;

namespace Celeste.Options.Internals
{
    public class OptionStructs
    {
        [Serializable]
        public struct BoolOptionDTO
        {
            public string name;
            public bool value;

            public BoolOptionDTO(BoolOption boolOption)
            {
                name = boolOption.name;
                value = boolOption.Value;
            }
        }

        [Serializable]
        public struct FloatOptionDTO
        {
            public string name;
            public float value;

            public FloatOptionDTO(FloatOption floatValue)
            {
                name = floatValue.name;
                value = floatValue.Value;
            }
        }

        [Serializable]
        public struct IntOptionDTO
        {
            public string name;
            public int value;

            public IntOptionDTO(IntOption intValue)
            {
                name = intValue.name;
                value = intValue.Value;
            }
        }

        [Serializable]
        public struct StringOptionDTO
        {
            public string name;
            public string value;

            public StringOptionDTO(StringOption stringValue)
            {
                name = stringValue.name;
                value = stringValue.Value;
            }
        }
    }
}
