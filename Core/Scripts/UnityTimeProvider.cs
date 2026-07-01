using Celeste.Core.Interfaces;
using UnityEngine;

namespace Celeste.Core
{
    public struct UnityTimeProvider : ITimeProvider
    {
        public float DeltaTime => Time.deltaTime;
    }
}