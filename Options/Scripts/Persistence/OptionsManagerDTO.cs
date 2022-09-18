using System;
using System.Collections.Generic;
using static Celeste.Options.Internals.OptionStructs;

namespace Celeste.Options.Internals
{
    [Serializable]
    public class OptionsManagerDTO
    {
        public List<BoolOption> boolOptions = new List<BoolOption>();
        public List<FloatOption> floatOptions = new List<FloatOption>();
        public List<IntOption> intOptions = new List<IntOption>();
        public List<StringOption> stringOptions = new List<StringOption>();

        public OptionsManagerDTO(OptionsRecord optionsRecord)
        {
            for (int i = 0, n = optionsRecord.NumBoolOptions; i < n; ++i)
            {
                boolOptions.Add(new BoolOption(optionsRecord.GetBoolOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumFloatOptions; i < n; ++i)
            {
                floatOptions.Add(new FloatOption(optionsRecord.GetFloatOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumIntOptions; i < n; ++i)
            {
                intOptions.Add(new IntOption(optionsRecord.GetIntOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumStringOptions; i < n; ++i)
            {
                stringOptions.Add(new StringOption(optionsRecord.GetStringOption(i)));
            }
        }
    }
}
