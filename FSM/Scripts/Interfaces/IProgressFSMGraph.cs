namespace Celeste.FSM
{
    public interface IProgressFSMGraph : IFSMGraph
    {
        FSMNode FinishNode { get; set; }
    }
}