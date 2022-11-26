using Celeste.BoardGame.Components;
using Celeste.Components;
using UnityEngine;

namespace Celeste.BoardGame.Tokens
{
    [CreateAssetMenu(fileName = nameof(Token), menuName = "Celeste/Board Game/Tokens/Token")]
    public class Token : ComponentContainerUsingSubAssets<TokenComponent>
    {
    }
}
