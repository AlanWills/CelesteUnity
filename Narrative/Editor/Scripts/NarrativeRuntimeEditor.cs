using Celeste.Narrative;
using UnityEditor;

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(NarrativeRuntime))]
    public class NarrativeRuntimeEditor : ILinearRuntimeEditor<NarrativeGraph>
    {
    }
}
