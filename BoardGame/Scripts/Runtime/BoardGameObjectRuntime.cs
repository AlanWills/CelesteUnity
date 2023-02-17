using Celeste.BoardGame.Components;
using Celeste.Components;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameObjectRuntime : ComponentContainerRuntime<BoardGameObjectComponent>
    {
        #region Properties and Fields

        public int InstanceId { get; }
        public int Guid => boardGameObject.Guid;
        public string Name => boardGameObject.name;

        private BoardGameObject boardGameObject;
        private static int currentInstanceId = 0;

        #endregion

        public BoardGameObjectRuntime(BoardGameObject boardGameObject)
        {
            this.boardGameObject = boardGameObject;
            InstanceId = ++currentInstanceId;

            InitializeComponents(boardGameObject);
        }
    }
}
