using UnityEngine;

namespace Celeste.BoardGame.Interfaces
{
    public interface IBoardGameLocations
    {
        Transform FindLocation(string name);
    }
}
