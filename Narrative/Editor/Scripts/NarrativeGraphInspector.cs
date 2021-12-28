using Celeste.FSM;
using Celeste.Narrative;
using CelesteEditor.FSM;
using CelesteEditor.Tools;
using CelesteEditor.Validation.GUIs;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.Narrative
{
    [CustomEditor(typeof(NarrativeGraph))]
    public class NarrativeGraphInspector : GlobalGraphEditor
    {
        #region Properties and Fields

        private ValidatorGUI<NarrativeGraph> narrativeValidatorGUI = new ValidatorGUI<NarrativeGraph>();
        private FSMGraphInspectorUtils fsmGraphInspectorUtils = new FSMGraphInspectorUtils();

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            NarrativeGraph graph = target as NarrativeGraph;

            narrativeValidatorGUI.GUI(graph);

            base.OnInspectorGUI();

            fsmGraphInspectorUtils.GUI(graph);
        }

        #endregion
    }
}
