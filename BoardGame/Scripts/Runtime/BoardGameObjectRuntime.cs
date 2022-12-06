using Celeste.BoardGame.Components;
using Celeste.Components;

namespace Celeste.BoardGame.Runtime
{
    public class BoardGameObjectRuntime : ComponentContainerRuntime<BoardGameObjectComponent>
    {
        #region Properties and Fields

        public int Guid => boardGameObject.Guid;
        public string Name => boardGameObject.name;

        private BoardGameObject boardGameObject;

        #endregion

        public BoardGameObjectRuntime(BoardGameObject boardGameObject)
        {
            this.boardGameObject = boardGameObject;

            InitializeComponents(boardGameObject);
        }

        public void SetDefaultValues()
        {
            SetComponentDefaultValues();
        }
    }
}
