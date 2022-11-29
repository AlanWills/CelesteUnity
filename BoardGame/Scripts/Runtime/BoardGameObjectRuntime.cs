using Celeste.BoardGame.Components;
using Celeste.Components;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameObjectRuntime : ComponentContainerRuntime<BoardGameObjectComponent>
    {
        public BoardGameObjectRuntime(BoardGameObject boardGameObject)
        {
            InitComponents(boardGameObject);
        }
    }
}
