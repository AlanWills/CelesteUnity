using Celeste.Tools.Attributes.GUI;
using System;
using UnityEngine;
using MinAttribute = Celeste.Tools.Attributes.GUI.MinAttribute;

namespace Celeste.UI.Layout
{
    [Serializable]
    public enum LayoutType
    {
        FixedRowCount,
        FixedColumnCount,
        Square
    }

    [Serializable]
    public enum SquareType
    {
        RowsThenColumns,
        ColumnsThenRows
    }

    [Serializable]
    public enum HorizontalAnchor
    {
        Left,
        Centre,
        Right,
    }

    [Serializable]
    public enum VerticalAnchor
    {
        Top,
        Centre,
        Bottom
    }

    [AddComponentMenu("Celeste/UI/Layout/Grid Layout Container")]
    public class GridLayoutContainer : MonoBehaviour, ILayoutContainer
    {
        #region Properties and Fields

        [SerializeField] private LayoutType layoutType;
        [SerializeField] private HorizontalAnchor horizontalAnchor = HorizontalAnchor.Centre;
        [SerializeField] private VerticalAnchor verticalAnchor = VerticalAnchor.Centre;
        [SerializeField, Min(1), ShowIfEnum(nameof(layoutType), (int)LayoutType.FixedRowCount)] private int fixedRowCount = 1;
        [SerializeField, Min(1), ShowIfEnum(nameof(layoutType), (int)LayoutType.FixedColumnCount)] private int fixedColumnCount = 1;
        [SerializeField, ShowIfEnum(nameof(layoutType), (int)LayoutType.Square)] private SquareType squareType;
        [SerializeField] private Vector2 cellSize = Vector2.one;
        [SerializeField] private bool reverseXDirection = false;
        [SerializeField] private bool reverseYDirection = false;

        #endregion

        public void OnChildAdded(GameObject gameObject)
        {
            Layout();
        }

        public void Layout()
        {
            switch (layoutType)
            {
                case LayoutType.FixedRowCount:
                    LayoutRows();
                    break;

                case LayoutType.FixedColumnCount:
                    LayoutColumns();
                    break;

                case LayoutType.Square:
                    LayoutSquare();
                    break;
            }
        }

        private void LayoutRows()
        {
            int xDirection = reverseXDirection ? -1 : 1;
            int yDirection = reverseYDirection ? -1 : 1;
            int childCount = transform.childCount;

            int numColumns = Mathf.CeilToInt(childCount / (float)fixedRowCount);
            float xHorizontalAnchor = (1 - numColumns) * cellSize.x * 0.5f * (int)horizontalAnchor;
            float yHorizontalAnchor = (1 - fixedRowCount) * cellSize.y * 0.5f * (int)verticalAnchor;

            for (int i = 0; i < childCount; ++i)
            {
                var child = transform.GetChild(i);
                Vector3 localPosition = new Vector3(
                    xDirection * (xHorizontalAnchor + i / fixedRowCount) * cellSize.x,
                    yDirection * (yHorizontalAnchor + i % fixedRowCount) * cellSize.y,
                    0);

                child.transform.localPosition = localPosition;
            }
        }

        private void LayoutColumns()
        {
            int xDirection = reverseXDirection ? -1 : 1;
            int yDirection = reverseYDirection ? -1 : 1;
            int childCount = transform.childCount;

            int numRows = Mathf.CeilToInt(childCount / (float)fixedColumnCount);
            float xHorizontalAnchor = (1 - fixedColumnCount) * cellSize.x * 0.5f * (int)horizontalAnchor;
            float yHorizontalAnchor = (1 - numRows) * cellSize.y * 0.5f * (int)verticalAnchor;

            for (int i = 0; i < childCount; ++i)
            {
                var child = transform.GetChild(i);
                Vector3 localPosition = new Vector3(
                    xDirection * (xHorizontalAnchor + i % fixedColumnCount) * cellSize.x,
                    yDirection * (yHorizontalAnchor + i / fixedColumnCount) * cellSize.y,
                    0);

                child.transform.localPosition = localPosition;
            }
        }

        private void LayoutSquare()
        {
            int xDirection = reverseXDirection ? -1 : 1;
            int yDirection = reverseYDirection ? -1 : 1;
            int childCount = transform.childCount;
            
            float sqrRootChildCount = Mathf.Sqrt(childCount);
            int numRows, numColumns;

            if (squareType == SquareType.RowsThenColumns)
            {
                numRows = Mathf.CeilToInt(sqrRootChildCount);
                numColumns = Mathf.CeilToInt(childCount / (float)numRows);
            }
            else
            {
                numColumns = Mathf.CeilToInt(sqrRootChildCount);
                numRows = Mathf.CeilToInt(childCount / (float)numColumns);
            }

            float xHorizontalAnchor = (1 - numColumns) * cellSize.x * 0.5f * (int)horizontalAnchor;
            float yHorizontalAnchor = (1 - numRows) * cellSize.y * 0.5f * (int)verticalAnchor;

            for (int i = 0; i < childCount; ++i)
            {
                int xOffset = squareType == SquareType.RowsThenColumns ? i / numRows : i % numColumns;
                int yOffset = squareType == SquareType.RowsThenColumns ? i % numRows : i / numColumns;
                var child = transform.GetChild(i);

                Vector3 localPosition = new Vector3(
                    xDirection * (xHorizontalAnchor + xOffset * cellSize.x),
                    yDirection * (yHorizontalAnchor + yOffset * cellSize.y),
                    0);

                child.transform.localPosition = localPosition;
            }
        }
    }
}
