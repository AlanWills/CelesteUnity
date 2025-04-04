using System;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Tools
{
    public static class GUIExtensions
    {
        public enum ListLayoutOptions
        {
            None = 0,
            AutomaticallyVerticalScope = 1
        }

        public static uint UIntField(string label, uint currentValue)
        {
            return (uint)Mathf.Max(0, IntField(label, (int)currentValue));
        }

        public static uint UIntField(uint currentValue)
        {
            return UIntField(string.Empty, currentValue);
        }

        public static int IntField(int currentInt)
        {
            return IntField(string.Empty, currentInt);
        }

        public static int IntField(string label, int currentInt)
        {
            using (new HorizontalScope())
            {
                if (!string.IsNullOrEmpty(label))
                {
                    Label(label, MaxWidth(150));
                    FlexibleSpace();
                }

                string currentIntText = currentInt.ToString();
                currentIntText = TextField(currentIntText, MinWidth(40), ExpandWidth(true));

                if (GUI.changed)
                {
                    if (int.TryParse(currentIntText, out int newInt))
                    {
                        currentInt = newInt;
                    }
                }

                return currentInt;
            }
        }

        public static float FloatField(float currentFloat)
        {
            return FloatField(string.Empty, currentFloat);
        }

        public static float FloatField(string label, float currentFloat)
        {
            using (new HorizontalScope())
            {
                if (!string.IsNullOrEmpty(label))
                {
                    Label(label, MaxWidth(150));
                    FlexibleSpace();
                }

                string currentFloatText = currentFloat.ToString();
                currentFloatText = TextField(currentFloatText, MinWidth(40));

                if (GUI.changed)
                {
                    if (float.TryParse(currentFloatText, out float newFloat))
                    {
                        currentFloat = newFloat;
                    }
                }

                return currentFloat;
            }
        }

        public static int PlusMinusField(string label, int currentInt)
        {
            using (new HorizontalScope())
            {
                Label($"{label}: {currentInt}");

                if (Button("+", ExpandWidth(false)))
                {
                    ++currentInt;
                }

                if (Button("-", ExpandWidth(false)))
                {
                    --currentInt;
                }
            }

            return currentInt;
        }

        public static int PaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem,
            Action addItem,
            Action<int> removeItem,
            Func<int, bool> filter = null)
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
                removeItem,
                filter);
        }

        public static int ReadOnlyPaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem,
            Func<int, bool> filter = null)
        {
            return PaginatedList(
                currentPage,
                entriesPerPage,
                numItems,
                drawItem,
                () => { return false; },
                () => { return false; },
                () => { },
                (i) => { },
                filter);
        }

        public static int PaginatedList(
            int currentPage,
            int entriesPerPage,
            int numItems,
            Action<int> drawItem,
            Func<bool> drawAddItem,
            Func<bool> drawRemoveItem,
            Action addItem,
            Action<int> removeItem,
            Func<int, bool> filter = null,
            ListLayoutOptions layoutOptions = ListLayoutOptions.AutomaticallyVerticalScope)
        {
            int numFilteredItems = 0;

            for (int i = 0; i < numItems; ++i)
            {
                if (filter == null || filter.Invoke(i))
                {
                    ++numFilteredItems;
                }
            }

            int numPages = Mathf.Max(1, Mathf.CeilToInt((float)numFilteredItems / entriesPerPage));

            if (currentPage >= numPages)
            {
                // We passed in a current page that is out of range, presumably because our filter has changed
                // Start again at 0
                currentPage = 0;
            }

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
            int unfilteredStartingIndex = currentPage * entriesPerPage;

            // We have a filter active, so we have to figure out what unfiltered absolute index corresponds to the start of our filtered index page
            if (numFilteredItems != numItems)
            {
                int foundFilteredItems = 0;
                unfilteredStartingIndex = 0;

                while (unfilteredStartingIndex < numItems && foundFilteredItems < currentPage * entriesPerPage)
                {
                    if (filter.Invoke(unfilteredStartingIndex))
                    {
                        ++foundFilteredItems;
                    }

                    ++unfilteredStartingIndex;
                }
            }

            for (int unfilteredIndex = unfilteredStartingIndex, filteredItemCount = 0; unfilteredIndex < numItems && filteredItemCount < Mathf.Min(entriesPerPage, numFilteredItems); ++unfilteredIndex)
            {
                if (filter != null && !filter.Invoke(unfilteredIndex))
                {
                    continue;
                }

                ++filteredItemCount;

                using (var horizontal = new HorizontalScope())
                {
                    if (drawRemoveItem())
                    {
                        removeIndex = unfilteredIndex;
                    }

                    if ((layoutOptions & ListLayoutOptions.AutomaticallyVerticalScope) == ListLayoutOptions.AutomaticallyVerticalScope)
                    {
                        using (var vertical = new VerticalScope())
                        {
                            drawItem(unfilteredIndex);
                        }
                    }
                    else
                    {
                        drawItem(unfilteredIndex);
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