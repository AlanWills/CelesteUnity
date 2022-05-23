using Celeste.FSM.Nodes.Events;
using Celeste.Loading.Events;
using Celeste.Loading.Parameters;

namespace Celeste.Loading.Nodes
{
    [CreateNodeMenu("Celeste/Loading/Execute Load Job")]
    public class ExecuteLoadJobNode : ParameterisedEventRaiserNode<LoadJob, LoadJobValue, LoadJobReference, LoadJobEvent>
    {
    }
}