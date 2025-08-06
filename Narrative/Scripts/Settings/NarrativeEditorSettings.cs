using System;
using System.Collections.Generic;
using Celeste.Events;
using Celeste.FSM;
using Celeste.FSM.Prefabs;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Tokens;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using Celeste.Tools.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Celeste.Narrative.Settings
{
    [CreateAssetMenu(
        fileName = nameof(NarrativeEditorSettings), 
        menuName = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM + "Narrative Editor Settings",
        order = CelesteMenuItemConstants.NARRATIVE_MENU_ITEM_PRIORITY)]
    public class NarrativeEditorSettings : EditorSettings<NarrativeEditorSettings>
    {
        #region ShortcutForPrefab

        [Serializable]
        public struct ShortcutForFSMNodePrefab
        {
            public FSMNodePrefab Prefab;
            public KeyCode ShortcutKey;
        }
        
        [Serializable]
        public struct ShortcutForNarrativeNodePrefab
        {
            public NarrativeNodePrefab Prefab;
            public KeyCode ShortcutKey;
        }

        #endregion
        
        #region Properties and Fields

        private const string FOLDER_PATH = "Assets/Narrative/Editor/Data/";
        public const string FILE_PATH = FOLDER_PATH + "NarrativeEditorSettings.asset";

        public IReadOnlyList<ShortcutForFSMNodePrefab> FSMNodePrefabsWithShortcuts => fsmNodePrefabsWithShortcuts;
        public IReadOnlyList<ShortcutForNarrativeNodePrefab> NarrativeNodePrefabsWithShortcuts => narrativeNodePrefabsWithShortcuts;

        [Header("Global Default Values")]
        public LocaTokens globalLocaTokens;

        [Header("Node Prefabs")]
        public NarrativeNodePrefab dialogueNodePrefab;
        public NarrativeNodePrefab narratorNodePrefab;
        public NarrativeNodePrefab choiceNodePrefab;
        public NarrativeNodePrefab timedChoiceNodePrefab;
        public FSMNodePrefab setBackgroundNodePrefab;
        
        [Header("Node Prefab Shortcuts")] 
        [SerializeField] private List<ShortcutForFSMNodePrefab> fsmNodePrefabsWithShortcuts = new();
        [SerializeField] private List<ShortcutForNarrativeNodePrefab> narrativeNodePrefabsWithShortcuts = new();
        
        #endregion

#if UNITY_EDITOR
        public static NarrativeEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            
            globalLocaTokens = EditorOnly.FindAsset<LocaTokens>("GlobalLocaTokens");
        }
#endif
    }
}