using Celeste.Assets;
using Celeste.DataStructures;
using Celeste.Log;
using Celeste.Options.Internals;
using Celeste.Parameters;
using Celeste.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Celeste.Options.Internals.OptionStructs;

namespace Celeste.Options
{
    [CreateAssetMenu(fileName = "New OptionsManager", menuName = "Celeste/Options/Options Manager")]
    public class OptionsManager : PersistentAddressableSingleton<OptionsManager, OptionsManagerDTO>
    {
        #region Properties and Fields

        private const string ADDRESS = "OptionsManager.asset";

        protected override string FilePath
        {
            get { return Path.Combine(Application.persistentDataPath, "OptionsManager.dat"); }
        }

        public int NumBoolOptions
        {
            get { return boolOptions.Count; }
        }

        public int NumFloatOptions
        {
            get { return floatOptions.Count; }
        }

        public int NumIntOptions
        {
            get { return intOptions.Count; }
        }

        public int NumStringOptions
        {
            get { return stringOptions.Count; }
        }

        [SerializeField]
        private List<BoolValue> boolOptions = new List<BoolValue>();

        [SerializeField]
        private List<FloatValue> floatOptions = new List<FloatValue>();

        [SerializeField]
        private List<IntValue> intOptions = new List<IntValue>();

        [SerializeField]
        private List<StringValue> stringOptions = new List<StringValue>();

        #endregion

        private OptionsManager() { }

        #region Save/Load Methods

        public static AsyncOperationHandleWrapper LoadAsync()
        {
            return LoadAsync(ADDRESS);
        }

        protected override OptionsManagerDTO Serialize()
        {
            return new OptionsManagerDTO(this);
        }

        protected override void Deserialize(OptionsManagerDTO optionsManagerDTO) 
        {
            Dictionary<string, BoolValue> boolValueLookup = boolOptions.ToNameLookup();
            foreach (BoolOption boolOption in optionsManagerDTO.boolOptions)
            {
                if (boolValueLookup.TryGetValue(boolOption.name, out BoolValue boolValue))
                {
                    boolValue.Value = boolOption.value;
                }
#if KEY_CHECKS
                else
                {
                    Debug.LogAssertionFormat("Could not find BoolValue {0}", boolOption.name);
                }
#endif
            }

            Dictionary<string, IntValue> intValueLookup = intOptions.ToNameLookup();
            foreach (IntOption intOption in optionsManagerDTO.intOptions)
            {
                if (intValueLookup.TryGetValue(intOption.name, out IntValue intValue))
                {
                    intValue.Value = intOption.value;
                }
#if KEY_CHECKS
                else
                {
                    Debug.LogAssertionFormat("Could not find IntValue {0}", intOption.name);
                }
#endif
            }

            Dictionary<string, FloatValue> floatValueLookup = floatOptions.ToNameLookup();
            foreach (FloatOption floatOption in optionsManagerDTO.floatOptions)
            {
                if (floatValueLookup.TryGetValue(floatOption.name, out FloatValue floatValue))
                {
                    floatValue.Value = floatOption.value;
                }
#if KEY_CHECKS
                else
                {
                    Debug.LogAssertionFormat("Could not find FloatValue {0}", floatOption.name);
                }
#endif
            }

            Dictionary<string, StringValue> stringValueLookup = stringOptions.ToNameLookup();
            foreach (StringOption stringOption in optionsManagerDTO.stringOptions)
            {
                if (stringValueLookup.TryGetValue(stringOption.name, out StringValue stringValue))
                {
                    stringValue.Value = stringOption.value;
                }
#if KEY_CHECKS
                else
                {
                    Debug.LogAssertionFormat("Could not find StringValue {0}", stringOption.name);
                }
#endif
            }
        }

        protected override void SetDefaultValues() { }

#endregion

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
    }
}