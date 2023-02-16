using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Tools
{
    public static class GUIUtils
    {
        public static int PaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem,
            Action addItem,
            Action<int> removeItem)
        {
            return PaginatedList(
                currentPage,
                entriesPerPage,
                numItems,
                drawItem,
                () =>
                {
                    using (var horizontal = new HorizontalScope())
                    {
                        FlexibleSpace();
                        return Button("+", ExpandWidth(false));
                    }
                },
                () => Button("-", ExpandWidth(false)),
                addItem,
                removeItem);
        }

        public static int ReadOnlyPaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem)
        {
            return PaginatedList(
                currentPage,
                entriesPerPage,
                numItems,
                drawItem,
                () => { return false; },
                () => { return false; },
                () => { },
                (i) => { });
        }

        public static int PaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem,
            Func<bool> drawAddItem,
            Func<bool> drawRemoveItem,
            Action addItem,
            Action<int> removeItem)
        {
            int numPages = Mathf.CeilToInt((float)numItems / entriesPerPage);

            using (HorizontalScope horizontal = new HorizontalScope())
            {
                // Fast Back
                using (var guiEnabled = new GUIEnabledScope(currentPage > 0))
                {
                    if (Button("<<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 5);
                    }
                }

                // Back
                using (var guiEnabled = new GUIEnabledScope(currentPage > 0))
                {
                    if (Button("<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 1);
                    }
                }

                Label($"{currentPage + 1} / {numPages}", GUI.skin.label.New().UpperCentreAligned());

                // Forward
                using (var guiEnabled = new GUIEnabledScope(currentPage < numPages - 1))
                {
                    if (Button(">", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages - 1, currentPage + 1);
                    }
                }

                // Fast Forward
                using (var guiEnabled = new GUIEnabledScope(currentPage < numPages - 1))
                {
                    if (Button(">>", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages - 1, currentPage + 5);
                    }
                }
            }

            int removeIndex = -1;
            int startingIndex = currentPage * entriesPerPage;

            for (int i = startingIndex; i < Mathf.Min(startingIndex + entriesPerPage, numItems); ++i)
            {
                using (var horizontal = new HorizontalScope())
                {
                    if (drawRemoveItem())
                    {
                        removeIndex = i;
                    }

                    using (var vertical = new VerticalScope())
                    {
                        drawItem(i);
                    }
                }
            }

            if (removeIndex >= 0)
            {
                removeItem(removeIndex);
            }

            if (drawAddItem())
            {
                addItem();
            }

            return currentPage;
        }
    }
}