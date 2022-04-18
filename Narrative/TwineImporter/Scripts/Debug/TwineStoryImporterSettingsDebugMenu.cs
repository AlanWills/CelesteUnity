using Celeste.Assets;
using Celeste.Debug.Menus;
using Celeste.Narrative.TwineImporter.Assets;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.Debug
{
    [CreateAssetMenu(fileName = nameof(TwineStoryImporterSettingsDebugMenu), menuName = "Celeste/Twine/Debug/Twine Story Importer Settings Debug Menu")]
    public class TwineStoryImporterSettingsDebugMenu : DebugMenu, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryImporterSettingsAssetReference twineStoryImporterSettings;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return twineStoryImporterSettings.ShouldLoad;
        }

        public IEnumerator LoadAssets()
        {
            yield return twineStoryImporterSettings.LoadAssetAsync();
        }

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            GUILayout.Label(twineStoryImporterSettings.Asset.IgnoreTag);
            GUILayout.Label(twineStoryImporterSettings.Asset.StartTag);

            foreach (var twineNodeParserStep in twineStoryImporterSettings.Asset.ParserSteps)
            {
                bool doesUseKeys = twineNodeParserStep is IUsesKeys;
                bool doesUseTags = twineNodeParserStep is IUsesTags;

                // Don't show anything unless we actually have keys or tags
                if (!doesUseKeys && !doesUseTags)
                {
                    continue;
                }

                GUILayout.Label(twineNodeParserStep.name, CelesteGUIStyles.BoldLabel);

                if (doesUseKeys)
                {
                    IUsesKeys usesKeys = twineNodeParserStep as IUsesKeys;

                    foreach (string key in usesKeys.Keys)
                    {
                        GUILayout.Label($"  {key}");
                    }
                }

                if (doesUseTags)
                {
                    IUsesTags usesTags = twineNodeParserStep as IUsesTags;

                    foreach (string tag in usesTags.Tags)
                    {
                        GUILayout.Label($"  {tag}");
                    }
                }
            }
        }

        #endregion
    }
}
