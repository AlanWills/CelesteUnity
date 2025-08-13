using Celeste.FSM;
using Celeste.Narrative;
using System;
using System.Reflection;
using Celeste.FSM.Prefabs;
using Celeste.Narrative.Nodes.Events;
using Celeste.Narrative.Settings;
using Celeste.Objects;
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
            return typeof(FSMNode).IsAssignableFrom(type) && 
                   !type.IsAbstract && 
                   type.IsClass && 
                   NodeEditorUtilities.GetAttrib(type, out Node.CreateNodeMenuAttribute _) &&
                   type.GetCustomAttribute<ObsoleteAttribute>() == null ? base.GetNodeMenuName(type) : null;
        }

        public override void AddContextMenuItems(GenericMenu menu)
        { 
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
            
            base.AddContextMenuItems(menu);
            
            menu.AddItem(new GUIContent("Celeste/Narrative/Dialogue"), false, () =>
            {
                CreateDialogueNode(pos);
            });
            menu.AddItem(new GUIContent("Celeste/Narrative/Narrator"), false, () =>
            {
                CreateNarratorNode(pos);
            });
            menu.AddItem(new GUIContent("Celeste/Narrative/Choice"), false, () =>
            {
                CreateChoiceNode(pos);
            });
            menu.AddItem(new GUIContent("Celeste/Narrative/Timed Choice"), false, () =>
            {
                CreateTimedChoiceNode(pos);
            });
            menu.AddItem(new GUIContent("Celeste/Narrative/Set Background"), false, () =>
            {
                CreateSetBackgroundNode(pos);
            });
            
            NarrativeEditorSettings narrativeEditorSettings = NarrativeEditorSettings.GetOrCreateSettings();
            foreach (var fsmNodePrefabWithShortcut in narrativeEditorSettings.FSMNodePrefabsWithShortcuts)
            {
                string menuName = ObjectNames.NicifyVariableName(fsmNodePrefabWithShortcut.Prefab.name);
                menu.AddItem(new GUIContent($"Celeste/Narrative/Prefabs/{menuName} ({fsmNodePrefabWithShortcut.ShortcutKey})"), false,
                    () =>
                    {
                        TryCreateNodeFromPrefab(fsmNodePrefabWithShortcut.Prefab, pos);    
                    });
            }
            
            foreach (var narrativeNodePrefabWithShortcut in narrativeEditorSettings.NarrativeNodePrefabsWithShortcuts)
            {
                string menuName = ObjectNames.NicifyVariableName(narrativeNodePrefabWithShortcut.Prefab.name);
                menu.AddItem(new GUIContent($"Celeste/Narrative/Prefabs/{menuName} ({narrativeNodePrefabWithShortcut.ShortcutKey})"), false,
                    () =>
                    {
                        TryCreateNodeFromPrefab(narrativeNodePrefabWithShortcut.Prefab, pos);    
                    });
            }
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

            if (e.keyCode == KeyCode.None)
            {
                return;
            }
            
            var narrativeEditorSettings = NarrativeEditorSettings.GetOrCreateSettings();
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(e.mousePosition);
            
            foreach (var shortcutAndFSMPrefabs in narrativeEditorSettings.FSMNodePrefabsWithShortcuts)
            {
                if (DoesEventMatchRequirements(e, shortcutAndFSMPrefabs.ShortcutKey) &&
                    TryCreateNodeFromPrefab(shortcutAndFSMPrefabs.Prefab, pos))
                {
                    e.Use();
                }
            }

            foreach (var shortcutAndNarrativePrefabs in narrativeEditorSettings.NarrativeNodePrefabsWithShortcuts)
            {
                if (DoesEventMatchRequirements(e, shortcutAndNarrativePrefabs.ShortcutKey) &&
                    TryCreateNodeFromPrefab(shortcutAndNarrativePrefabs.Prefab, pos))
                {
                    e.Use();
                }
            }
        }

        private bool DoesEventMatchRequirements(Event e, KeyCode keyCode)
        {
            return e.keyCode == keyCode && e.modifiers == EventModifiers.None;
        }

        private void CreateDialogueNode(Vector2 pos)
        {
            NarrativeNodePrefab nodePrefab = NarrativeEditorSettings.GetOrCreateSettings().dialogueNodePrefab;
            if (!TryCreateNodeFromPrefab(nodePrefab, pos))
            {
                CreateNode(typeof(DialogueNode), pos);
            }
        }

        private void CreateNarratorNode(Vector2 pos)
        {
            NarrativeNodePrefab nodePrefab = NarrativeEditorSettings.GetOrCreateSettings().narratorNodePrefab;
            if (!TryCreateNodeFromPrefab(nodePrefab, pos))
            {
                CreateNode(typeof(NarrativeNode), pos);
            }
        }

        private void CreateChoiceNode(Vector2 pos)
        {
            NarrativeNodePrefab nodePrefab = NarrativeEditorSettings.GetOrCreateSettings().choiceNodePrefab;
            if (!TryCreateNodeFromPrefab(nodePrefab, pos))
            {
                CreateNode(typeof(ChoiceNode), pos);
            }
        }

        private void CreateTimedChoiceNode(Vector2 pos)
        {
            NarrativeNodePrefab nodePrefab = NarrativeEditorSettings.GetOrCreateSettings().timedChoiceNodePrefab;
            if (!TryCreateNodeFromPrefab(nodePrefab, pos))
            {
                CreateNode(typeof(TimedChoiceNode), pos);
            }
        }

        private void CreateSetBackgroundNode(Vector2 pos)
        {
            FSMNodePrefab nodePrefab = NarrativeEditorSettings.GetOrCreateSettings().setBackgroundNodePrefab;
            if (!TryCreateNodeFromPrefab(nodePrefab, pos))
            {
                CreateNode(typeof(SetBackgroundEventRaiserNode), pos);
            }
        }

        private bool TryCreateNodeFromPrefab<T>(AssetWrapperScriptableObject<T> prefab, Vector2 pos) where T : FSMNode
        {
            if (prefab != null && prefab.asset != null)
            {
                FSMNode node = CreateNode(prefab.asset.GetType(), pos) as FSMNode;
                node.CopyInGraph(prefab.asset);
                EditorOnly.SetDirty(node);
                return true;
            }

            return false;
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
