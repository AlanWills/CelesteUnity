using Celeste.Assets;
using Celeste.Options.Internals;
using Celeste.Options.Settings;
using Celeste.Persistence;
using System.Collections;
using UnityEngine;
using static Celeste.Options.Internals.OptionStructs;

namespace Celeste.Options
{
    [AddComponentMenu("Celeste/Options/Options Manager")]
    public class OptionsManager : PersistentSceneManager<OptionsManager, OptionsManagerDTO>, IHasAssets
    {
        #region Properties and Fields

        public const string FILE_NAME = "Options.dat";

        protected override string FileName => FILE_NAME;

        [SerializeField] private OptionsSettings optionsSettings;
        [SerializeField] private OptionsRecord optionsRecord;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return optionsSettings.ShouldLoadAssets();
        }

        public IEnumerator LoadAssets()
        {
            yield return optionsSettings.LoadAssets();

            Load();
        }

        #endregion

        #region Save/Load Methods

        protected override OptionsManagerDTO Serialize()
        {
            return new OptionsManagerDTO(optionsRecord);
        }

        protected override void Deserialize(OptionsManagerDTO optionsManagerDTO) 
        {
            AddOptions();

            foreach (BoolOptionDTO boolOption in optionsManagerDTO.boolOptions)
            {
                optionsRecord.SetOption(boolOption.name, boolOption.value);
            }

            foreach (IntOptionDTO intOption in optionsManagerDTO.intOptions)
            {
                optionsRecord.SetOption(intOption.name, intOption.value);
            }

            foreach (FloatOptionDTO floatOption in optionsManagerDTO.floatOptions)
            {
                optionsRecord.SetOption(floatOption.name, floatOption.value);
            }

            foreach (StringOptionDTO stringOption in optionsManagerDTO.stringOptions)
            {
                optionsRecord.SetOption(stringOption.name, stringOption.value);
            }
        }

        protected override void SetDefaultValues() 
        {
            AddOptions();
        }

        #endregion

        private void AddOptions()
        {
            optionsRecord.Initialize();
            
            foreach (var boolOption in optionsSettings.BoolOptions)
            {
                optionsRecord.AddOption(boolOption);
            }

            foreach (var floatOption in optionsSettings.FloatOptions)
            {
                optionsRecord.AddOption(floatOption);
            }

            foreach (var intOption in optionsSettings.IntOptions)
            {
                optionsRecord.AddOption(intOption);
            }

            foreach (var stringOption in optionsSettings.StringOptions)
            {
                optionsRecord.AddOption(stringOption);
            }
        }
    }
}