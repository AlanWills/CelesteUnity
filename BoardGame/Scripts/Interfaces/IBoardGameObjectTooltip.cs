using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame.Interfaces
{
    public interface IBoardGameObjectTooltip
    {
        void ShowTooltip(Instance instance, Vector3 position, bool isWorldSpace);
        void HideTooltip(Instance instance);
    }
}
