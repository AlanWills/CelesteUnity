using Celeste.FSM;
using CelesteEditor.Tools;
using CelesteEditor.Validation.GUIs;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(FSMGraph))]
    public class FSMGraphInspector : Editor
    {
        #region Properties and Fields

        private ValidatorGUI<FSMGraph> fsmValidatorGUI = new ValidatorGUI<FSMGraph>();
        private FSMGraphInspectorUtils fsmGraphInspectorUtils = new FSMGraphInspectorUtils();

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            FSMGraph graph = target as FSMGraph;

            fsmValidatorGUI.GUI(graph);

            base.OnInspectorGUI();

            fsmGraphInspectorUtils.GUI(graph);
        }

        #endregion
    }
}
