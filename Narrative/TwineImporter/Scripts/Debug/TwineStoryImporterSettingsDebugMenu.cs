using Celeste.Assets;
using Celeste.Debug.Menus;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.TwineImporter.Debug
{
    [CreateAssetMenu(fileName = nameof(TwineStoryImporterSettingsDebugMenu), order = CelesteMenuItemConstants.TWINE_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.TWINE_MENU_ITEM + "Debug/Twine Story Importer Settings Debug Menu")]
    public class TwineStoryImporterSettingsDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private TwineStoryImporterSettings twineStoryImporterSettings;

        #endregion

        #region GUI

        protected override void OnDrawMenu()
        {
            GUILayout.Label(twineStoryImporterSettings.IgnoreTag);
            GUILayout.Label(twineStoryImporterSettings.StartTag);

            foreach (var twineNodeParserStep in twineStoryImporterSettings.ParserSteps)
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
