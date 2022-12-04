using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectActor
    {
        GameObject InstantiateActor(Instance instance, Transform parent);

        string GetCurrentLocationName(Instance instance);
        void SetCurrentLocationName(Instance instance, string locationName);
    }
}
