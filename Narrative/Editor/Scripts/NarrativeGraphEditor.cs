using Celeste.FSM;
using Celeste.Narrative;
using System;
using Celeste.Events;
using Celeste.Narrative.Nodes.Events;
using Celeste.Narrative.Settings;
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
            
            menu.AddItem(new GUIContent("Celeste/Narrative/Set Background"), false, () =>
            {
                BackgroundEventRaiserNode backgroundEventRaiser = CreateNode(typeof(BackgroundEventRaiserNode), pos) as BackgroundEventRaiserNode;
                backgroundEventRaiser.toRaise = NarrativeEditorSettings.GetOrCreateSettings().defaultSetBackgroundEvent;
                EditorOnly.SetDirty(backgroundEventRaiser);
            });
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
