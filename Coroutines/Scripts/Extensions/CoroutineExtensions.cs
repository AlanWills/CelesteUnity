using System.Collections;
using System.Threading.Tasks;

namespace Celeste.Coroutines
{
    public static class CoroutineExtensions
    {
        public static IEnumerator ExecuteAsCoroutine(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
        
        public static IEnumerator ExecuteAsCoroutine(this ValueTask task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
        
        public static IEnumerator ExecuteAsCoroutine<T>(this Task<T> task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
        
        public static IEnumerator ExecuteAsCoroutine<T>(this ValueTask<T> task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
        }
    }
}