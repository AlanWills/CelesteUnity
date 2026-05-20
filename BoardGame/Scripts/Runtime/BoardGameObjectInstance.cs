using Celeste.BoardGame.Components;
using Celeste.Components;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameObjectInstance : ComponentContainerInstance<BoardGameObjectComponent>
    {
        #region Properties and Fields

        public int Guid => boardGameObject.Guid;
        public string Name => boardGameObject.name;

        private readonly BoardGameObject boardGameObject;

        #endregion

        public BoardGameObjectInstance(BoardGameObject boardGameObject)
        {
            this.boardGameObject = boardGameObject;

            InitializeComponents(boardGameObject);
        }
    }
}
