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

        public OptionsManagerDTO(OptionsManager optionsManager)
        {
            for (int i = 0, n = optionsManager.NumBoolOptions; i < n; ++i)
            {
                boolOptions.Add(new BoolOption(optionsManager.GetBoolOption(i)));
            }

            for (int i = 0, n = optionsManager.NumFloatOptions; i < n; ++i)
            {
                floatOptions.Add(new FloatOption(optionsManager.GetFloatOption(i)));
            }

            for (int i = 0, n = optionsManager.NumIntOptions; i < n; ++i)
            {
                intOptions.Add(new IntOption(optionsManager.GetIntOption(i)));
            }

            for (int i = 0, n = optionsManager.NumStringOptions; i < n; ++i)
            {
                stringOptions.Add(new StringOption(optionsManager.GetStringOption(i)));
            }
        }
    }
}
