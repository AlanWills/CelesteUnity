using System.Threading.Tasks;
using Celeste.Events;

namespace Celeste.Loading
{
    public class TaskEventRaiser : ParameterisedEventRaiser<Task, TaskEvent> { }
}