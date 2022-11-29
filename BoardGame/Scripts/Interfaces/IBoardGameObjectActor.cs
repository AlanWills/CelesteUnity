using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectActor
    {
        GameObject InstantiateActor(BoardGame boardGame, Instance instance);
    }
}
