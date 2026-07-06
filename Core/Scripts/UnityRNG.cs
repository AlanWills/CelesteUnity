using Celeste.Core.Interfaces;

namespace Celeste.Core
{
    public readonly struct UnityRNG : IRNG
    {
        public int FromRangeInclusive(int inclusiveMin, int inclusiveMax)
        {
            return UnityEngine.Random.Range(inclusiveMin, inclusiveMax + 1);
        }
    }
}