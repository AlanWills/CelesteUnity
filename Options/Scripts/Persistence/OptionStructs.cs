using Celeste.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celeste.Options.Internals
{
    public class OptionStructs
    {
        [Serializable]
        public struct BoolOption
        {
            public string name;
            public bool value;

            public BoolOption(BoolValue boolValue)
            {
                name = boolValue.name;
                value = boolValue.Value;
            }
        }

        [Serializable]
        public struct FloatOption
        {
            public string name;
            public float value;

            public FloatOption(FloatValue floatValue)
            {
                name = floatValue.name;
                value = floatValue.Value;
            }
        }

        [Serializable]
        public struct IntOption
        {
            public string name;
            public int value;

            public IntOption(IntValue intValue)
            {
                name = intValue.name;
                value = intValue.Value;
            }
        }

        [Serializable]
        public struct StringOption
        {
            public string name;
            public string value;

            public StringOption(StringValue stringValue)
            {
                name = stringValue.name;
                value = stringValue.Value;
            }
        }
    }
}
