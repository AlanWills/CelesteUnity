using Celeste.Application;
using Celeste.DataStructures;
using Celeste.Parameters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = nameof(OptionsRecord), menuName = CelesteMenuItemConstants.OPTIONS_MENU_ITEM + "Record", order = CelesteMenuItemConstants.OPTIONS_MENU_ITEM_PRIORITY)]
    public class OptionsRecord : ScriptableObject
    {
        #region Properties and Fields

        public int NumBoolOptions => boolOptions.Count;
        public int NumFloatOptions => floatOptions.Count;
        public int NumIntOptions => intOptions.Count;
        public int NumStringOptions => stringOptions.Count;

        private ApplicationPlatform PlatformForOptions
        {
            get
            {
                if (isMobilePlatform.Value)
                {
                    return ApplicationPlatform.Mobile;
                }

                return ApplicationPlatform.Computer;
            }
        }

        [SerializeField] private BoolValue isMobilePlatform;
        [SerializeField] private Events.Event saveOptions;

        [NonSerialized] private List<BoolOption> boolOptions = new List<BoolOption>();
        [NonSerialized] private List<FloatOption> floatOptions = new List<FloatOption>();
        [NonSerialized] private List<IntOption> intOptions = new List<IntOption>();
        [NonSerialized] private List<StringOption> stringOptions = new List<StringOption>();

        #endregion

        public void Initialize()
        {
            boolOptions.Clear();
            floatOptions.Clear();
            intOptions.Clear();
            stringOptions.Clear();
        }

        public void AddOption(BoolOption boolOption)
        {
            boolOption.SetDefaultValue(PlatformForOptions);
            boolOption.AddValueChangedCallback((args) => OnOptionChanged());
            UnityEngine.Debug.Assert(!boolOptions.Exists(x => string.CompareOrdinal(x.DisplayName, boolOption.DisplayName) == 0), $"{nameof(BoolOption)} {boolOption.DisplayName} already exists in Options.");
            boolOptions.Add(boolOption);
        }

        public void AddOption(FloatOption floatOption)
        {
            floatOption.SetDefaultValue(PlatformForOptions);
            floatOption.AddValueChangedCallback((args) => OnOptionChanged());
            UnityEngine.Debug.Assert(!boolOptions.Exists(x => string.CompareOrdinal(x.DisplayName, floatOption.DisplayName) == 0), $"{nameof(FloatOption)} {floatOption.DisplayName} already exists in Options.");
            floatOptions.Add(floatOption);
        }

        public void AddOption(IntOption intOption)
        {
            intOption.SetDefaultValue(PlatformForOptions);
            intOption.AddValueChangedCallback((args) => OnOptionChanged());
            UnityEngine.Debug.Assert(!boolOptions.Exists(x => string.CompareOrdinal(x.DisplayName, intOption.DisplayName) == 0), $"{nameof(IntOption)} {intOption.DisplayName} already exists in Options.");
            intOptions.Add(intOption);
        }

        public void AddOption(StringOption stringOption)
        {
            stringOption.SetDefaultValue(PlatformForOptions);
            stringOption.AddValueChangedCallback((args) => OnOptionChanged());
            UnityEngine.Debug.Assert(!boolOptions.Exists(x => string.CompareOrdinal(x.DisplayName, stringOption.DisplayName) == 0), $"{nameof(StringOption)} {stringOption.DisplayName} already exists in Options.");
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

        public void ResetAll()
        {
            var applicationPlatform = PlatformForOptions;

            foreach (BoolOption boolOption in boolOptions)
            {
                boolOption.SetDefaultValue(applicationPlatform);
            }

            foreach (IntOption intOption in intOptions)
            {
                intOption.SetDefaultValue(applicationPlatform);
            }

            foreach (FloatOption floatOption in floatOptions)
            {
                floatOption.SetDefaultValue(applicationPlatform);
            }

            foreach (StringOption stringOption in stringOptions)
            {
                stringOption.SetDefaultValue(applicationPlatform);
            }

            saveOptions.Invoke();
        }

        private void OnOptionChanged()
        {
            saveOptions.Invoke();
        }
    }
}
