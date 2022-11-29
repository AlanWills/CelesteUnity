using Celeste.DataStructures;
using Celeste.Scene.Hierarchy;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.BoardGame
{
    [CreateAssetMenu(fileName = nameof(BoardGame), menuName = "Celeste/Board Game/Board Game")]
    public class BoardGame : ScriptableObject
    {
        #region Properties and Fields

        public int NumBoardGameObjects => boardGameObjects.Count;

        [SerializeField] private GameObject board;
        [SerializeField] private StringGameObjectDictionary boardLocations;
        [SerializeField] private List<BoardGameObject> boardGameObjects = new List<BoardGameObject>();

        #endregion

        public GameObject InstantiateBoard(Transform parent)
        {
            return Instantiate(board, parent);
        }

        public BoardGameObject GetBoardGameObject(int index)
        {
            return boardGameObjects.Get(index);
        }

        public GameObject FindBoardLocation(string boardLocationName)
        {
            return boardLocations.GetItem(boardLocationName);
        }
    }
}
