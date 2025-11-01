using System.Threading.Tasks;
using Celeste.Events;

namespace Celeste.Loading
{
    public class TaskEventListener : ParameterisedEventListener<Task, TaskEvent, TaskUnityEvent> { }
}