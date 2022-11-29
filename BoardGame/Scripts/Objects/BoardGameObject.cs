using Celeste.BoardGame.Components;
using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(fileName = nameof(BoardGameObject), menuName = "Celeste/Board Game/Objects/Board Game Object")]
    public class BoardGameObject : ComponentContainerUsingSubAssets<BoardGameObjectComponent>
    {
    }
}
