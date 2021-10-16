using Celeste.FSM;
using Celeste.Narrative;
using Celeste.Narrative.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Wizards
{
    [Serializable]
    public class TwineStory
    {
        public List<TwineNode> passages = new List<TwineNode>();
    }

    [Serializable]
    public struct TwineNode
    {
        public string name;
        public string text;
    }

    public class TwineJsonImporter : ScriptableWizard
    {
        #region Properties and Fields

        private FSMGraph narrativeGraph;
        private string filePath;

        #endregion

        #region Menu Item

        [MenuItem("Celeste/Narrative/Twine Json Importer")]
        public static void CreateTwineJsonImporterWizard()
        {
            DisplayWizard<TwineJsonImporter>("Twine Json Importer", "Import");
        }

        #endregion

        #region Wizard Methods

        protected override bool DrawWizardGUI()
        {
            bool propertiesModified = base.DrawWizardGUI();

            EditorGUI.BeginChangeCheck();

            narrativeGraph = EditorGUILayout.ObjectField(narrativeGraph, typeof(FSMGraph), false) as FSMGraph;

            using (GUILayout.HorizontalScope horizontalScope = new GUILayout.HorizontalScope())
            {
                filePath = EditorGUILayout.TextField("File Path", filePath);

                if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                {
                    filePath = EditorUtility.OpenFilePanel("Choose Json File", "", "json");
                }
            }

            return propertiesModified || EditorGUI.EndChangeCheck();
        }

        private void OnWizardCreate()
        {
            string fileContents = File.ReadAllText(filePath);
            TwineStory twineStory = new TwineStory();
            JsonUtility.FromJsonOverwrite(fileContents, twineStory);

            Vector2 position = Vector2.zero;

            foreach (TwineNode node in twineStory.passages)
            {
                DialogueNode dialogueNode = narrativeGraph.AddNode<DialogueNode>();
                dialogueNode.RawDialogue = node.text;
                dialogueNode.UIPosition = UIPosition.Centre;
                dialogueNode.position = position;
                dialogueNode.name = node.name;

                AssetDatabase.AddObjectToAsset(dialogueNode, narrativeGraph);

                position += new Vector2(300, 0);
            }

            EditorUtility.SetDirty(narrativeGraph);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
