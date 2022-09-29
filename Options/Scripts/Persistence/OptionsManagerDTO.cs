using System;
using System.Collections.Generic;
using static Celeste.Options.Internals.OptionStructs;

namespace Celeste.Options.Internals
{
    [Serializable]
    public class OptionsManagerDTO
    {
        public List<BoolOptionDTO> boolOptions = new List<BoolOptionDTO>();
        public List<FloatOptionDTO> floatOptions = new List<FloatOptionDTO>();
        public List<IntOptionDTO> intOptions = new List<IntOptionDTO>();
        public List<StringOptionDTO> stringOptions = new List<StringOptionDTO>();

        public OptionsManagerDTO(OptionsRecord optionsRecord)
        {
            for (int i = 0, n = optionsRecord.NumBoolOptions; i < n; ++i)
            {
                boolOptions.Add(new BoolOptionDTO(optionsRecord.GetBoolOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumFloatOptions; i < n; ++i)
            {
                floatOptions.Add(new FloatOptionDTO(optionsRecord.GetFloatOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumIntOptions; i < n; ++i)
            {
                intOptions.Add(new IntOptionDTO(optionsRecord.GetIntOption(i)));
            }

            for (int i = 0, n = optionsRecord.NumStringOptions; i < n; ++i)
            {
                stringOptions.Add(new StringOptionDTO(optionsRecord.GetStringOption(i)));
            }
        }
    }
}
