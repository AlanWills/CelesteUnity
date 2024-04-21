using Celeste.Assets;
using Celeste.Options.Internals;
using Celeste.Options.Settings;
using Celeste.Persistence;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            AddOptions(
                optionsManagerDTO.boolOptions.ToDictionary(x => x.name, x => x.value),
                optionsManagerDTO.floatOptions.ToDictionary(x => x.name, x => x.value),
                optionsManagerDTO.intOptions.ToDictionary(x => x.name, x => x.value),
                optionsManagerDTO.stringOptions.ToDictionary(x => x.name, x => x.value));
        }

        protected override void SetDefaultValues() 
        {
            AddOptions();
        }

        #endregion

        private void AddOptions()
        {
            optionsRecord.Initialize(
                optionsSettings.BoolOptions, 
                optionsSettings.FloatOptions, 
                optionsSettings.IntOptions, 
                optionsSettings.StringOptions);
        }

        private void AddOptions(
            IReadOnlyDictionary<string, bool> boolOptionValues,
            IReadOnlyDictionary<string, float> floatOptionValues,
            IReadOnlyDictionary<string, int> intOptionValues,
            IReadOnlyDictionary<string, string> stringOptionValues)
        {
            optionsRecord.Initialize(
                optionsSettings.BoolOptions,
                boolOptionValues,
                optionsSettings.FloatOptions,
                floatOptionValues,
                optionsSettings.IntOptions,
                intOptionValues,
                optionsSettings.StringOptions,
                stringOptionValues);
        }
    }
}