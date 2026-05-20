using Celeste.BoardGame.Runtime;

namespace Celeste.BoardGame
{
    public interface IBoardGameObjectComponentView
    {
        void Hookup(BoardGameObjectInstance boardGameObjectInstance);
        void Shutdown();
    }
}
