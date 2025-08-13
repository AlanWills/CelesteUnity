using Celeste.FSM.Nodes.Events;
using Celeste.Narrative;
using Celeste.Narrative.Nodes.Events;
using Celeste.Tools;
using CelesteEditor.FSM.Nodes;

namespace CelesteEditor.Narrative
{
    [CustomNodeEditor(typeof(SetBackgroundEventRaiserNode))]
    public class SetBackgroundEventRaiserNodeEditor : FSMNodeEditor
    {
        #region GUI

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            SetBackgroundEventRaiserNode setBackgroundNode = target as SetBackgroundEventRaiserNode;
            bool isBackgroundSetBefore = setBackgroundNode.argument.Background != null;
            
            DrawFixGUI();
            DrawDefaultPortPair();
            DrawEventNodeValues();
            serializedObject.ApplyModifiedProperties();

            if (!isBackgroundSetBefore && setBackgroundNode.argument.Background != null)
            {
                setBackgroundNode.argument.Offset = setBackgroundNode.argument.Background.DefaultOffset;
                EditorOnly.SetDirty(setBackgroundNode);
            }
        }

        private void DrawEventNodeValues()
        {
            string[] excludes = { "m_Script", "graph", "position", "ports" };
            DrawNodeProperties(serializedObject, excludes);
        }

        #endregion
    }
}