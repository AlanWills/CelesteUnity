using Celeste.Events;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Tokens;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
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
        public LocaTokens globalLocaTokens;
        public BackgroundEvent defaultSetBackgroundEvent;
        public bool hasAddDialogueNodeShortcut = true;
        [ShowIf("hasAddDialogueNodeShortcut")] public KeyCode addDialogueNodeShortcutKey = KeyCode.D;
        public bool hasAddNarratorNodeShortcut = true;
        [ShowIf("hasAddNarratorNodeShortcut")] public KeyCode addNarratorNodeShortcutKey = KeyCode.N;
        public bool hasAddChoiceNodeShortcut = true;
        [ShowIf("hasAddChoiceNodeShortcut")] public KeyCode addChoiceNodeShortcutKey = KeyCode.C;
        public bool hasAddTimedChoiceNodeShortcut = true;
        [ShowIf("hasAddTimedChoiceNodeShortcut")] public KeyCode addTimedChoiceNodeShortcutKey = KeyCode.T;
        public bool hasAddSetBackgroundNodeShortcut = true;
        [ShowIf("hasAddSetBackgroundNodeShortcut")] public KeyCode addSetBackgroundNodeShortcutKey = KeyCode.B;

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
            globalLocaTokens = EditorOnly.FindAsset<LocaTokens>("GlobalLocaTokens");
            defaultSetBackgroundEvent = EditorOnly.FindAsset<BackgroundEvent>("SetBackground");
        }
#endif
    }
}