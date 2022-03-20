using Celeste.Narrative;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Celeste.Narrative.Characters;
using static UnityEditor.EditorGUILayout;
using Celeste.Logic;
using CelesteEditor.Tools;
using Celeste.Twine;
using Celeste.Inventory;
using static Celeste.Narrative.TwineImporter.TwineStoryImporterSettings;
using Celeste.Narrative.TwineImporter;
using Celeste;

namespace CelesteEditor.Narrative.TwineImporter
{
    public class TwineStoryImporterWizard : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField] private TwineStory twineStory;
        [SerializeField] private TwineStoryImporterSettings importerSettings;
        [SerializeField] private NarrativeGraph narrativeGraph;
        [SerializeField] private bool stopOnParserError;

        private TwineStoryAnalysis twineStoryAnalysis;
        private List<string> removedUnresolvedTags = new List<string>();
        private List<string> removedUnresolvedKeys = new List<string>();

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Narrative/Twine Story Importer")]
        public static void ShowTwineStoryImporterWizard()
        {
            DisplayWizard<TwineStoryImporterWizard>("Twine Story Importer", "Close", "Import");
        }

        #endregion

        #region Wizard Methods

        protected override bool DrawWizardGUI()
        {
            bool changed = base.DrawWizardGUI();

            if (twineStory != null && importerSettings != null && twineStoryAnalysis == null)
            {
                AnalyseStory();
            }

            Space();

            using (HorizontalScope horizontal = new HorizontalScope())
            {
                if (GUILayout.Button("Analyse", GUILayout.ExpandWidth(false)))
                {
                    AnalyseStory();
                }
            }

            if (twineStoryAnalysis != null)
            {
                DrawCharactersGUI();
                DrawLocaTokensGUI();
                DrawConditionsGUI();
                DrawParametersGUI();
                DrawBackgroundsGUI();
                DrawSubNarrativesGUI();
                DrawInventoryItemsGUI();
                DrawUnresolvedTagsGUI();
                DrawUnresolvedKeysGUI();
            }

            return changed;
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            // Clear the log before we start
            LogUtility.Clear();

            TwineStoryImporter.Import(
                twineStory, 
                importerSettings,
                narrativeGraph,
                stopOnParserError);

            EditorUtility.SetDirty(narrativeGraph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion

        #region GUI

        private void DrawCharactersGUI()
        {
            Space();
            LabelField("Characters", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundCharacter in twineStoryAnalysis.foundCharacters)
            {
                LabelField(foundCharacter);
            }
        }

        private void DrawLocaTokensGUI()
        {
            Space();
            LabelField("Loca Tokens", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundLocaToken in twineStoryAnalysis.foundLocaTokens)
            {
                LabelField(foundLocaToken);
            }
        }

        private void DrawConditionsGUI()
        {
            Space();
            LabelField("Conditions", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundCondition in twineStoryAnalysis.foundConditions)
            {
                LabelField(foundCondition);
            }
        }

        private void DrawParametersGUI()
        {
            Space();
            LabelField("Parameters", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundParameter in twineStoryAnalysis.foundParameters)
            {
                LabelField(foundParameter);
            }
        }

        private void DrawBackgroundsGUI()
        {
            Space();
            LabelField("Backgrounds", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundBackground in twineStoryAnalysis.foundBackgrounds)
            {
                LabelField(foundBackground);
            }
        }

        private void DrawSubNarrativesGUI()
        {
            Space();
            LabelField("Sub Narratives", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundSubNarrative in twineStoryAnalysis.foundSubNarratives)
            {
                LabelField(foundSubNarrative);
            }
        }

        private void DrawInventoryItemsGUI()
        {
            Space();
            LabelField("Inventory Items", CelesteGUIStyles.BoldLabel);
            Space();

            foreach (string foundInventoryItem in twineStoryAnalysis.foundInventoryItems)
            {
                LabelField(foundInventoryItem);
            }
        }

        private void DrawUnresolvedTagsGUI()
        {
            if (twineStoryAnalysis.unrecognizedTags.Count > 0)
            {
                Space();
                LabelField("Unresolved Tags", CelesteGUIStyles.BoldLabel);
                Space();

                foreach (string unresolvedTag in twineStoryAnalysis.unrecognizedTags)
                {
                    using (HorizontalScope horizontal = new HorizontalScope())
                    {
                        LabelField(unresolvedTag);

                        if (GUILayout.Button("Find Or Create Character", GUILayout.ExpandWidth(false)))
                        {
                            if (FindOrCreateCharacterName(unresolvedTag))
                            {
                                removedUnresolvedTags.Add(unresolvedTag);
                                twineStoryAnalysis.foundCharacters.Add(unresolvedTag);
                            }
                        }
                    }
                }
            }

            foreach (string unresolvedTag in removedUnresolvedTags)
            {
                twineStoryAnalysis.unrecognizedTags.Remove(unresolvedTag);
            }
            removedUnresolvedTags.Clear();
        }

        private void DrawUnresolvedKeysGUI()
        {
            if (twineStoryAnalysis.unrecognizedKeys.Count > 0)
            {
                Space();
                LabelField("Unresolved Keys", CelesteGUIStyles.BoldLabel);
                Space();

                foreach (string unresolvedKey in twineStoryAnalysis.unrecognizedKeys)
                {
                    using (HorizontalScope horizontal = new HorizontalScope())
                    {
                        LabelField(unresolvedKey);

                        if (GUILayout.Button("Loca Token", GUILayout.ExpandWidth(false)))
                        {
                            if (FindLocaToken(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundLocaTokens.Add(unresolvedKey);
                            }
                        }

                        if (GUILayout.Button("Condition", GUILayout.ExpandWidth(false)))
                        {
                            if (FindCondition(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundConditions.Add(unresolvedKey);
                            }
                        }

                        if (GUILayout.Button("Parameter", GUILayout.ExpandWidth(false)))
                        {
                            if (FindParameter(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundParameters.Add(unresolvedKey);
                            }
                        }

                        if (GUILayout.Button("Background", GUILayout.ExpandWidth(false)))
                        {
                            if (FindBackground(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundBackgrounds.Add(unresolvedKey);
                            }
                        }

                        if (GUILayout.Button("Sub Narrative", GUILayout.ExpandWidth(false)))
                        {
                            if (FindSubNarrative(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundSubNarratives.Add(unresolvedKey);
                            }
                        }

                        if (GUILayout.Button("InventoryItem", GUILayout.ExpandWidth(false)))
                        {
                            if (FindInventoryItem(unresolvedKey))
                            {
                                removedUnresolvedTags.Add(unresolvedKey);
                                twineStoryAnalysis.foundInventoryItems.Add(unresolvedKey);
                            }
                        }
                    }
                }
            }

            foreach (string unresolvedKey in removedUnresolvedKeys)
            {
                twineStoryAnalysis.unrecognizedKeys.Remove(unresolvedKey);
            }
            removedUnresolvedKeys.Clear();
        }

        #endregion

        #region Utility

        private void AnalyseStory()
        {
            if (twineStoryAnalysis == null)
            {
                twineStoryAnalysis = new TwineStoryAnalysis();
            }

            importerSettings.Analyse(twineStory, twineStoryAnalysis);
        }

        private bool FindOrCreateCharacterName(string characterName)
        {
            if (!TryFind(characterName, importerSettings.CharactersDirectory, out Character character))
            {
                character = Character.Create(characterName, importerSettings.CharactersDirectory);
            }
            
            AddCharacterToSettings(character);
            return true;
        }

        private bool FindLocaToken(string locaTokenName)
        {
            if (TryFind(locaTokenName, importerSettings.LocaTokensDirectory, out ScriptableObject locaToken))
            {
                AddLocaTokenToSettings(locaToken);
                return true;
            }

            return false;
        }

        private bool FindCondition(string conditionName)
        {
            if (TryFind(conditionName, importerSettings.ConditionsDirectory, out Condition condition))
            {
                AddConditionToSettings(condition);
                return true;
            }

            return false;
        }

        private bool FindParameter(string parameterName)
        {
            if (TryFind(parameterName, importerSettings.ParametersDirectory, out ScriptableObject parameter))
            {
                AddParameterToSettings(parameter);
                return true;
            }

            return false;
        }

        private bool FindBackground(string backgroundName)
        {
            if (TryFind(backgroundName, importerSettings.BackgroundsDirectory, out Background background))
            {
                AddBackgroundToSettings(background);
                return true;
            }

            return false;
        }

        private bool FindSubNarrative(string subNarrativeName)
        {
            if (TryFind(subNarrativeName, importerSettings.SubNarrativesDirectory, out NarrativeGraph subNarrative))
            {
                AddSubNarrativeToSettings(subNarrative);
                return true;
            }

            return false;
        }

        private bool FindInventoryItem(string inventoryItemName)
        {
            if (TryFind(inventoryItemName, importerSettings.InventoryItemsDirectory, out InventoryItem inventoryItem))
            {
                AddInventoryItemToSettings(inventoryItem);
                return true;
            }

            return false;
        }

        private bool TryFind<T>(string name, string directory, out T asset) where T : Object
        {
            asset = AssetUtility.FindAsset<T>(name, directory);
            return asset != null;
        }

        private void AddCharacterToSettings(Character character)
        {
            importerSettings.characterTags.Add(new CharacterTag(character.CharacterName, character));
            RemoveUnresolvedTag(character.CharacterName);
        }

        private void AddLocaTokenToSettings(ScriptableObject locaToken)
        {
            importerSettings.locaTokenKeys.Add(new LocaTokenKey(locaToken.name, locaToken));
            RemoveUnresolvedKey(locaToken.name);
        }

        private void AddConditionToSettings(Condition condition)
        {
            importerSettings.conditionKeys.Add(new ConditionKey(condition.name, condition));
            RemoveUnresolvedKey(condition.name);
        }

        private void AddParameterToSettings(ScriptableObject parameter)
        {
            importerSettings.parameterKeys.Add(new ParameterKey(parameter.name, parameter));
            RemoveUnresolvedKey(parameter.name);
        }

        private void AddBackgroundToSettings(Background background)
        {
            importerSettings.backgroundKeys.Add(new BackgroundKey(background.name, background));
            RemoveUnresolvedKey(background.name);
        }

        private void AddSubNarrativeToSettings(NarrativeGraph subNarrative)
        {
            importerSettings.subNarrativeKeys.Add(new SubNarrativeKey(subNarrative.name, subNarrative));
            RemoveUnresolvedKey(subNarrative.name);
        }

        private void AddInventoryItemToSettings(InventoryItem inventoryItem)
        {
            importerSettings.inventoryItemKeys.Add(new InventoryItemKey(inventoryItem.name, inventoryItem));
            RemoveUnresolvedKey(inventoryItem.name);
        }

        private void RemoveUnresolvedTag(string tag)
        {
            removedUnresolvedTags.Add(tag);

            EditorUtility.SetDirty(importerSettings);
            AssetDatabase.SaveAssets();
        }

        private void RemoveUnresolvedKey(string key)
        {
            removedUnresolvedKeys.Add(key);

            EditorUtility.SetDirty(importerSettings);
            AssetDatabase.SaveAssets();
        }

        #endregion
    }
}
