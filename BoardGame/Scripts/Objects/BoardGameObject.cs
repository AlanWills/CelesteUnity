using Celeste.BoardGame.Components;
using Celeste.Components;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(fileName = nameof(BoardGameObject), menuName = "Celeste/Board Game/Objects/Board Game Object")]
    public class BoardGameObject : ComponentContainerUsingSubAssets<BoardGameObjectComponent>, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private int guid;

        #endregion
    }
}
