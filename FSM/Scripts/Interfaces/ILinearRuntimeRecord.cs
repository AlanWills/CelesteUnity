namespace Celeste.FSM
{
    public interface ILinearRuntimeRecord
    {
        FSMGraphNodePath CurrentNodePath { get; set; }
    }

    public class EmptyLinearRuntimeRecord : ILinearRuntimeRecord
    {
        FSMGraphNodePath ILinearRuntimeRecord.CurrentNodePath
        {
            get => new FSMGraphNodePath();
            set { }
        }
    }
}