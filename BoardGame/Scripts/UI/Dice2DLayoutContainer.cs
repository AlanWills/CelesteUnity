using Celeste.Tools.Attributes.GUI;
using Celeste.UI;
using System;
using UnityEngine;
using MinAttribute = Celeste.Tools.Attributes.GUI.MinAttribute;

namespace Celeste.BoardGame.UI
{
    [Serializable]
    public enum Dice2DLayoutType
    {
        FixedRowCount,
        FixedColumnCount
    }

    [AddComponentMenu("Celeste/Board Game/UI/Dice 2D Layout Container")]
    public class Dice2DLayoutContainer : MonoBehaviour, ILayoutContainer
    {
        #region Properties and Fields

        [SerializeField] private Dice2DLayoutType layoutType;
        [SerializeField, Min(1), ShowIfEnum(nameof(layoutType), (int)Dice2DLayoutType.FixedRowCount)] private int fixedRowCount = 1;
        [SerializeField, Min(1), ShowIfEnum(nameof(layoutType), (int)Dice2DLayoutType.FixedColumnCount)] private int fixedColumnCount = 1;
        [SerializeField] private Vector2 cellSize = new Vector2(100, 100);
        [SerializeField] private bool reverseXDirection = false;
        [SerializeField] private bool reverseYDirection = false;

        #endregion

        public void OnChildAdded(GameObject gameObject)
        {
            switch (layoutType)
            {
                case Dice2DLayoutType.FixedRowCount:
                    LayoutRows();
                    break;

                case Dice2DLayoutType.FixedColumnCount:
                    LayoutColumns();
                    break;
            }
        }

        private void LayoutRows()
        {
            int xDirection = reverseXDirection ? -1 : 1;
            int yDirection = reverseYDirection ? -1 : 1;

            for (int i = 0, n = transform.childCount; i < n; ++i)
            {
                var child = transform.GetChild(i);
                Vector3 localPosition = new Vector3(
                    xDirection * (i / fixedRowCount) * cellSize.x,
                    yDirection * (i % fixedRowCount) * cellSize.y,
                    0);

                child.transform.localPosition = localPosition;
            }
        }

        private void LayoutColumns()
        {
            int xDirection = reverseXDirection ? -1 : 1;
            int yDirection = reverseYDirection ? -1 : 1;

            for (int i = 0, n = transform.childCount; i < n; ++i)
            {
                var child = transform.GetChild(i);
                Vector3 localPosition = new Vector3(
                    xDirection * (i % fixedColumnCount) * cellSize.x,
                    yDirection * (i / fixedColumnCount) * cellSize.y,
                    0);

                child.transform.localPosition = localPosition;
            }
        }
    }
}
