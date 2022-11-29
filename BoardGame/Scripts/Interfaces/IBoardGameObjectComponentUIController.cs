using Celeste.BoardGame.Runtime;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectComponentUIController
    {
        void Hookup(BoardGameObjectRuntime boardGameObjectRuntime);
        void Shutdown();
    }
}
