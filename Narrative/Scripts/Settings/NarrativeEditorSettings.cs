using Celeste.Narrative.Characters;
using Celeste.Tools;
using Celeste.Tools.Settings;
using UnityEngine;

namespace Celeste.Narrative.Settings
{
    [CreateAssetMenu(
        fileName = nameof(NarrativeEditorSettings), 
        menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Narrative Editor Settings",
        order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeEditorSettings : EditorSettings<NarrativeEditorSettings>
    {
        #region Properties and Fields

        private const string FOLDER_PATH = "Assets/Narrative/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "NarrativeEditorSettings.asset";

        public Character narratorCharacter;

        #endregion

#if UNITY_EDITOR
        public static NarrativeEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            narratorCharacter = EditorOnly.FindAsset<Character>("Narrator");
        }
#endif
    }
}