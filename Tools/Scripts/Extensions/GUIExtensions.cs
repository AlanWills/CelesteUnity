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
                currentIntText = TextField(currentIntText, MinWidth(40));

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
            if (filter != null)
            {
                for (int i = 0, n = numItems; i < n; ++i)
                {
                    if (!filter(i))
                    {
                        --numItems;
                    }
                }
            }

            int numPages = Mathf.Max(1, Mathf.CeilToInt((float)numItems / entriesPerPage));

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

            for (int i = startingIndex, filteredMessageIndex = 0; filteredMessageIndex < Mathf.Min(startingIndex + entriesPerPage, numItems); ++i)
            {
                if (filter != null && !filter.Invoke(i))
                {
                    continue;
                }

                ++filteredMessageIndex;

                using (var horizontal = new HorizontalScope())
                {
                    if (drawRemoveItem())
                    {
                        removeIndex = i;
                    }

                    if ((layoutOptions & ListLayoutOptions.AutomaticallyVerticalScope) == ListLayoutOptions.AutomaticallyVerticalScope)
                    {
                        using (var vertical = new VerticalScope())
                        {
                            drawItem(i);
                        }
                    }
                    else
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