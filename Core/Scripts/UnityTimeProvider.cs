using Celeste.Core.Interfaces;
using UnityEngine;

namespace Celeste.Core
{
    public struct UnityTimeProvider : ITimeProvider
    {
        public float Time => UnityEngine.Time.time;
        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}