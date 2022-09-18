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

        private List<BoolValue> boolOptions = new List<BoolValue>();
        private List<FloatValue> floatOptions = new List<FloatValue>();
        private List<IntValue> intOptions = new List<IntValue>();
        private List<StringValue> stringOptions = new List<StringValue>();

        #endregion

        public void AddOption(BoolValue boolOption)
        {
            boolOption.AddValueChangedCallback((args) => OnOptionChanged());
            boolOptions.Add(boolOption);
        }

        public void AddOption(FloatValue floatOption)
        {
            floatOption.AddValueChangedCallback((args) => OnOptionChanged());
            floatOptions.Add(floatOption);
        }

        public void AddOption(IntValue intOption)
        {
            intOption.AddValueChangedCallback((args) => OnOptionChanged());
            intOptions.Add(intOption);
        }

        public void AddOption(StringValue stringOption)
        {
            stringOption.AddValueChangedCallback((args) => OnOptionChanged());
            stringOptions.Add(stringOption);
        }

        public BoolValue GetBoolOption(int index)
        {
            return boolOptions.Get(index);
        }

        public FloatValue GetFloatOption(int index)
        {
            return floatOptions.Get(index);
        }

        public IntValue GetIntOption(int index)
        {
            return intOptions.Get(index);
        }

        public StringValue GetStringOption(int index)
        {
            return stringOptions.Get(index);
        }

        public BoolValue FindBoolOption(string boolOptionName)
        {
            return boolOptions.Find(x => string.CompareOrdinal(x.name, boolOptionName) == 0);
        }

        public FloatValue FindFloatOption(string floatOptionName)
        {
            return floatOptions.Find(x => string.CompareOrdinal(x.name, floatOptionName) == 0);
        }

        public IntValue FindIntOption(string intOptionName)
        {
            return intOptions.Find(x => string.CompareOrdinal(x.name, intOptionName) == 0);
        }

        public StringValue FindStringOption(string stringOptionName)
        {
            return stringOptions.Find(x => string.CompareOrdinal(x.name, stringOptionName) == 0);
        }

        public void SetOption(string optionName, bool value)
        {
            BoolValue boolOption = FindBoolOption(optionName);

#if NULL_CHECKS
            if (boolOption != null)
#endif
            {
                boolOption.Value = value;
            }
        }

        public void SetOption(string floatOptionName, float value)
        {
            FloatValue floatOption = FindFloatOption(floatOptionName);

#if NULL_CHECKS
            if (floatOption != null)
#endif
            {
                floatOption.Value = value;
            }
        }

        public void SetOption(string optionName, int value)
        {
            IntValue intOption = FindIntOption(optionName);

#if NULL_CHECKS
            if (intOption != null)
#endif
            {
                intOption.Value = value;
            }
        }

        public void SetOption(string optionName, string value)
        {
            StringValue stringOption = FindStringOption(optionName);

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
