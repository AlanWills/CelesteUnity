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
            return PaginatedListImpl(
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
            return PaginatedListImpl(
                currentPage,
                entriesPerPage,
                numItems,
                drawItem,
                () => { return false; },
                () => { return false; },
                () => { },
                (i) => { });
        }

        public static int PaginatedListImpl(
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
                {
                    GUI.enabled = currentPage > 0;
                    if (Button("<<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 5);
                    }
                }

                // Back
                {
                    GUI.enabled = currentPage > 0;
                    if (Button("<", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Max(0, currentPage - 1);
                    }
                }

                FlexibleSpace();
                Label($"{currentPage + 1} / {numPages}", ExpandWidth(false));
                FlexibleSpace();

                // Forward
                {
                    GUI.enabled = currentPage < numPages - 1;
                    if (Button(">", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages, currentPage + 1);
                    }
                }

                // Fast Forward
                {
                    GUI.enabled = currentPage < numPages - 1;
                    if (Button(">>", ExpandWidth(false)))
                    {
                        currentPage = Mathf.Min(numPages, currentPage + 5);
                    }
                }

                GUI.enabled = true;
            }

            int removeIndex = -1;

            for (int i = 0; i < Mathf.Min(entriesPerPage, numItems); ++i)
            {
                int startingIndex = currentPage * entriesPerPage;

                using (var horizontal = new HorizontalScope())
                {
                    if (drawRemoveItem())
                    {
                        removeIndex = i;
                    }

                    drawItem(startingIndex + i);
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