using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.Choices;
using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using Celeste.DataStructures;
using Celeste.Narrative.Characters;
using static UnityEditor.EditorGUILayout;
using static CelesteEditor.Narrative.Twine.TwineStoryImporterSettings;
using CelesteEditor.Narrative.Nodes;
using CelesteEditor.Narrative.Twine.ParserSteps;

namespace CelesteEditor.Narrative.Twine
{
    public class TwineStoryImporter : ScriptableWizard
    {
        #region Properties and Fields

        [SerializeField] private TwineStory twineStory;
        [SerializeField] private NarrativeGraph narrativeGraph;
        [SerializeField] private TwineStoryImporterSettings importerSettings;

        private HashSet<string> missingCharacterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private List<string> removedCharacterNames = new List<string>();

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Narrative/Twine Story Importer")]
        public static void CreateTwineStoryImporterWizard()
        {
            DisplayWizard<TwineStoryImporter>("Twine Story Importer", "Close", "Import");
        }

        #endregion

        #region Wizard Methods

        protected override bool DrawWizardGUI()
        {
            bool changed = base.DrawWizardGUI();
            
            changed |= DrawCharactersGUI();

            return changed;
        }

        private void OnWizardCreate()
        {
            Close();
        }

        private void OnWizardOtherButton()
        {
            Dictionary<int, FSMNode> nodeLookup = new Dictionary<int, FSMNode>();
            Vector2 startNodePosition = twineStory.passages.Find(x => x.pid == twineStory.startnode).position;

            foreach (TwineNode twineNode in twineStory.passages)
            {
                if (importerSettings.TryParse(
                    twineNode,
                    narrativeGraph,
                    startNodePosition, 
                    out FSMNode fsmNode))
                {
                    nodeLookup.Add(twineNode.pid, fsmNode);
                    AssetDatabase.AddObjectToAsset(fsmNode, narrativeGraph);
                }
            }

            // Now resolve transitions
            foreach (TwineNode node in twineStory.passages)
            {
                if (node.links != null && node.links.Length > 0)
                {
                    FSMNode graphNode = nodeLookup[node.pid];

                    if (graphNode is ChoiceNode)
                    {
                        foreach (TwineNodeLink link in node.links)
                        {
                            Debug.Assert(nodeLookup.ContainsKey(link.pid), $"Could not find node with pid {link.pid} for link on node {graphNode.name}.");
                            FSMNode target = nodeLookup[link.pid];

                            NodePort outputPort = graphNode.GetOutputPort(link.link);
                            Debug.Assert(outputPort != null, $"Could not find output port {link.link} in node {graphNode.name}.");

                            NodePort inputPort = target.GetDefaultInputPort();
                            Debug.Assert(inputPort != null, $"Could not find default input port in node {target.name}.");

                            outputPort.Connect(inputPort);
                        }
                    }
                    else if (graphNode is DialogueNode)
                    {
                        foreach (TwineNodeLink link in node.links)
                        {
                            Debug.Assert(nodeLookup.ContainsKey(link.pid), $"Could not find node with pid {link.pid} for link on node {graphNode.name}.");
                            FSMNode target = nodeLookup[link.pid];

                            NodePort outputPort = graphNode.GetDefaultOutputPort();
                            Debug.Assert(outputPort != null, $"Could not find default output port in node {graphNode.name}.");

                            NodePort inputPort = target.GetDefaultInputPort();
                            Debug.Assert(inputPort != null, $"Could not find default input port in node {target.name}.");

                            outputPort.Connect(inputPort);
                        }
                    }
                }
            }

            // Set the start node using the pid from the twine story
            narrativeGraph.startNode = nodeLookup[twineStory.startnode];

            EditorUtility.SetDirty(narrativeGraph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion

        #region GUI

        private bool DrawCharactersGUI()
        {
            bool changed = false;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Characters", CelesteEditorStyles.BoldLabel);
            EditorGUILayout.Space();

            using (HorizontalScope horizontal = new HorizontalScope())
            {
                if (GUILayout.Button("Detect Missing Characters In Settings", GUILayout.ExpandWidth(false)))
                {
                    missingCharacterNames.Clear();

                    foreach (var node in twineStory.passages)
                    {
                        foreach (string tag in node.tags)
                        {
                            if (importerSettings.CouldBeUnregisteredCharacterTag(tag))
                            {
                                changed = true;
                                missingCharacterNames.Add(tag);
                            }
                        }
                    }
                }

                if (GUILayout.Button("Find Or Create All Missing Characters", GUILayout.ExpandWidth(false)))
                {
                    foreach (string missingCharacterName in missingCharacterNames)
                    {
                        FindOrCreateCharacterName(missingCharacterName);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Missing In Settings:", CelesteEditorStyles.BoldLabel);
            EditorGUILayout.Space();

            foreach (string missingCharacterName in missingCharacterNames)
            {
                using (EditorGUI.IndentLevelScope indent = new EditorGUI.IndentLevelScope())
                using (HorizontalScope horizontal = new HorizontalScope())
                {
                    EditorGUILayout.LabelField(missingCharacterName);

                    if (GUILayout.Button("Find Or Create", GUILayout.ExpandWidth(false)))
                    {
                        FindOrCreateCharacterName(missingCharacterName);
                    }
                }
            }

            if (removedCharacterNames.Count > 0)
            {
                foreach (string characterName in removedCharacterNames)
                {
                    missingCharacterNames.Remove(characterName);
                }

                removedCharacterNames.Clear();
            }

            return changed;
        }

        #endregion

        #region Utility

        private void FindOrCreateCharacterName(string characterName)
        {
            string[] characterGUIDs = AssetDatabase.FindAssets($"t:{nameof(Character)} {characterName}", new string[] { importerSettings.charactersDirectory });
            if (characterGUIDs != null && characterGUIDs.Length > 0)
            {
                if (characterGUIDs.Length == 1)
                {
                    string characterAssetPath = AssetDatabase.GUIDToAssetPath(characterGUIDs[0]);
                    Character character = AssetDatabase.LoadAssetAtPath<Character>(characterAssetPath);
                    Debug.Assert(character != null, $"Unable to load character {characterName} from path {characterAssetPath}.");
                    AddCharacterToSettings(character);
                }
                else
                {
                    Debug.LogAssertion($"Found more than one character matching name {characterName}.  Skipping find or add...");
                }
            }
            else
            {
                Character character = Character.Create(characterName, importerSettings.charactersDirectory);
                AddCharacterToSettings(character);
            }
        }

        private void AddCharacterToSettings(Character character)
        {
            importerSettings.characterTags.Add(new CharacterTag(character.CharacterName, character));
            removedCharacterNames.Add(character.CharacterName);
            
            EditorUtility.SetDirty(importerSettings);
            AssetDatabase.SaveAssets();
        }

        private void SetDialogueNodeValues(
            DialogueNode dialogueNode, 
            TwineNode node,
            Vector2 startNodePosition)
        {
            Character character = importerSettings.FindCharacterFromTag(node.tags);
            Debug.Assert(character != null, $"Could not find character for node {node.name} ({node.pid}).");
            UIPosition characterDefaultPosition = character != null ? character.DefaultUIPosition : UIPosition.Centre;

            DialogueNodeBuilder.
                        WithNode(dialogueNode).
                        WithName(node.name).
                        WithPosition((node.position - startNodePosition) * importerSettings.nodeSpread).
                        WithRawDialogue(node.text).
                        WithCharacter(character).
                        WithUIPosition(importerSettings.FindUIPositionFromTag(node.tags, characterDefaultPosition));
        }

        #endregion
    }
}
