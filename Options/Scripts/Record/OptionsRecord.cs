using Celeste.DataStructures;
using Celeste.Parameters;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(OptionsRecord), menuName = "Celeste/Options/Record")]
    public class OptionsRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumBoolOptions => boolOptions.Count;
        public int NumFloatOptions => floatOptions.Count;
        public int NumIntOptions => intOptions.Count;
        public int NumStringOptions => stringOptions.Count;

        [SerializeField] private Events.Event saveOptions;

        private List<BoolOption> boolOptions = new List<BoolOption>();
        private List<FloatOption> floatOptions = new List<FloatOption>();
        private List<IntOption> intOptions = new List<IntOption>();
        private List<StringOption> stringOptions = new List<StringOption>();

        #endregion

        public void AddOption(BoolOption boolOption)
        {
            boolOption.AddValueChangedCallback((args) => OnOptionChanged());
            boolOptions.Add(boolOption);
        }

        public void AddOption(FloatOption floatOption)
        {
            floatOption.AddValueChangedCallback((args) => OnOptionChanged());
            floatOptions.Add(floatOption);
        }

        public void AddOption(IntOption intOption)
        {
            intOption.AddValueChangedCallback((args) => OnOptionChanged());
            intOptions.Add(intOption);
        }

        public void AddOption(StringOption stringOption)
        {
            stringOption.AddValueChangedCallback((args) => OnOptionChanged());
            stringOptions.Add(stringOption);
        }

        public BoolOption GetBoolOption(int index)
        {
            return boolOptions.Get(index);
        }

        public FloatOption GetFloatOption(int index)
        {
            return floatOptions.Get(index);
        }

        public IntOption GetIntOption(int index)
        {
            return intOptions.Get(index);
        }

        public StringOption GetStringOption(int index)
        {
            return stringOptions.Get(index);
        }

        public BoolOption FindBoolOption(string boolOptionName)
        {
            return boolOptions.Find(x => string.CompareOrdinal(x.name, boolOptionName) == 0);
        }

        public FloatOption FindFloatOption(string floatOptionName)
        {
            return floatOptions.Find(x => string.CompareOrdinal(x.name, floatOptionName) == 0);
        }

        public IntOption FindIntOption(string intOptionName)
        {
            return intOptions.Find(x => string.CompareOrdinal(x.name, intOptionName) == 0);
        }

        public StringOption FindStringOption(string stringOptionName)
        {
            return stringOptions.Find(x => string.CompareOrdinal(x.name, stringOptionName) == 0);
        }

        public void SetOption(string optionName, bool value)
        {
            BoolOption boolOption = FindBoolOption(optionName);

#if NULL_CHECKS
            if (boolOption != null)
#endif
            {
                boolOption.Value = value;
            }
        }

        public void SetOption(string floatOptionName, float value)
        {
            FloatOption floatOption = FindFloatOption(floatOptionName);

#if NULL_CHECKS
            if (floatOption != null)
#endif
            {
                floatOption.Value = value;
            }
        }

        public void SetOption(string optionName, int value)
        {
            IntOption intOption = FindIntOption(optionName);

#if NULL_CHECKS
            if (intOption != null)
#endif
            {
                intOption.Value = value;
            }
        }

        public void SetOption(string optionName, string value)
        {
            StringOption stringOption = FindStringOption(optionName);

#if NULL_CHECKS
            if (stringOption != null)
#endif
            {
                stringOption.Value = value;
            }
        }

        private void OnOptionChanged()
        {
            saveOptions.Invoke();
        }
    }
}
