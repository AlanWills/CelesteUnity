using System.ComponentModel;
using Celeste.Tools;

namespace Celeste.Narrative
{
    [DisplayName("Finish Node")]
    [CreateNodeMenu("Celeste/Narrative/Finish")]
    [NodeTint(0, 0, 1f)]
    public class FinishNode : NarrativeNode
    {
        protected override void OnAddToGraph()
        {
            base.OnAddToGraph();

            if (graph is NarrativeGraph narrativeGraph)
            {
                narrativeGraph.FinishNode = this;
                EditorOnly.SetDirty(narrativeGraph);
            }
        }
    }
}