using Celeste.FSM;
using Celeste.Narrative;
using System;
using Celeste.Narrative.Nodes.Events;
using Celeste.Narrative.Settings;
using Celeste.Narrative.UI;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;
using Event = UnityEngine.Event;

namespace CelesteEditor.Narrative
{
    [CustomNodeGraphEditor(typeof(NarrativeGraph))]
    public class NarrativeGraphEditor : NodeGraphEditor
    {
        #region Context Menu

        public override string GetNodeMenuName(Type type)
        {
            return typeof(FSMNode).IsAssignableFrom(type) && !type.IsAbstract ? base.GetNodeMenuName(type) : null;
        }

        public override void AddContextMenuItems(GenericMenu menu)
        { 
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
            
            base.AddContextMenuItems(menu);
            
            menu.AddItem(new GUIContent("Celeste/Narrative/Narrator"), false, () =>
            {
                CreateNarratorNode(pos);
            });
            menu.AddItem(new GUIContent("Celeste/Narrative/Set Background"), false, () =>
            {
                CreateSetBackgroundNode(pos);
            });
        }

        #endregion
        
        #region GUI
        
        public override void OnGUI()
        {
            base.OnGUI();
            
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    HandleCommandEvent(e);
                    break;
            }
        }

        #endregion
        
        #region Utility
        
        private void HandleCommandEvent(Event e)
        {
            if (EditorGUIUtility.editingTextField)
            {
                return;
            }
            
            var narrativeEditorSettings = NarrativeEditorSettings.GetOrCreateSettings();
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(e.mousePosition);
            
            if (DoesEventMatchRequirements(e, narrativeEditorSettings.hasAddDialogueNodeShortcut, narrativeEditorSettings.addDialogueNodeShortcutKey))
            {
                CreateNode(typeof(DialogueNode), pos);
                e.Use();
            }
            else if (DoesEventMatchRequirements(e, narrativeEditorSettings.hasAddNarratorNodeShortcut,
                         narrativeEditorSettings.addNarratorNodeShortcutKey))
            {
                CreateNarratorNode(pos);
                e.Use();
            }
            else if (DoesEventMatchRequirements(e, narrativeEditorSettings.hasAddChoiceNodeShortcut, narrativeEditorSettings.addChoiceNodeShortcutKey))
            {
                CreateNode(typeof(ChoiceNode), pos);
                e.Use();
            }
            else if (DoesEventMatchRequirements(e, narrativeEditorSettings.hasAddTimedChoiceNodeShortcut, narrativeEditorSettings.addTimedChoiceNodeShortcutKey))
            {
                CreateNode(typeof(TimedChoiceNode), pos);
                e.Use();
            }
            else if (DoesEventMatchRequirements(e, narrativeEditorSettings.hasAddSetBackgroundNodeShortcut, narrativeEditorSettings.addSetBackgroundNodeShortcutKey))
            {
                CreateSetBackgroundNode(pos);
                e.Use();
            }
        }

        private bool DoesEventMatchRequirements(Event e, bool isEventActive, KeyCode keyCode)
        {
            return isEventActive && e.keyCode == keyCode && e.modifiers == EventModifiers.None;
        }

        private void CreateNarratorNode(Vector2 pos)
        {
            DialogueNode dialogueNode = CreateNode(typeof(DialogueNode), pos) as DialogueNode;
            dialogueNode.Character = NarrativeEditorSettings.GetOrCreateSettings().narratorCharacter;
            dialogueNode.UIPosition = UIPosition.Narrator;
            dialogueNode.DialogueType = DialogueType.Speech;
            EditorOnly.SetDirty(dialogueNode);
        }

        private void CreateSetBackgroundNode(Vector2 pos)
        {
            BackgroundEventRaiserNode backgroundEventRaiser = CreateNode(typeof(BackgroundEventRaiserNode), pos) as BackgroundEventRaiserNode;
            backgroundEventRaiser.toRaise = NarrativeEditorSettings.GetOrCreateSettings().defaultSetBackgroundEvent;
            EditorOnly.SetDirty(backgroundEventRaiser);
        }
        
        #endregion

        #region Add/Remove/Copy

        public override void RemoveNode(Node node)
        {
            base.RemoveNode(node);

            if (target.nodes.Count == 1)
            {
                (target as FSMGraph).startNode = target.nodes[0] as FSMNode;
            }
        }

        #endregion
    }
}
