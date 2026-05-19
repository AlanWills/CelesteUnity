using Celeste.BoardGame.Runtime;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectComponentView
    {
        void Hookup(BoardGameObjectRuntime boardGameObjectRuntime);
        void Shutdown();
    }
}
