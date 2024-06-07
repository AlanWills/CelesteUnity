using System;

namespace Celeste.Tools
{
    public class Semaphore
    {
        public bool Locked { get; set; }

        public SemaphoreScope Lock()
        {
            return new SemaphoreScope(this);
        }
    }

    public struct SemaphoreScope : IDisposable
    {
        private Semaphore semaphore;

        public SemaphoreScope(Semaphore semaphore)
        {
            this.semaphore = semaphore;
            this.semaphore.Locked = true;
        }

        public void Dispose()
        {
            semaphore.Locked = false;
        }
    }
}