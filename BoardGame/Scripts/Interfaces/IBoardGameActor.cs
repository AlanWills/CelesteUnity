using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame
{
    public interface IBoardGameActor
    {
        GameObject InstantiateActor(Instance instance, Transform parent);
    }
}
