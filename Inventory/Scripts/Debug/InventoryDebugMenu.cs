using Celeste.Assets;
using Celeste.Debug.Menus;
using Celeste.Inventory.AssetReferences;
using System.Collections;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Inventory.Debug
{
    [CreateAssetMenu(fileName = nameof(InventoryDebugMenu), menuName = "Celeste/Inventory/Debug/Inventory Debug Menu")]
    public class InventoryDebugMenu : DebugMenu, IHasAssets
    {
        #region Properties and Fields

        private int NumEvents
        {
            get { return inventoryItemCatalogue.Asset.NumItems; }
        }

        private int NumPages
        {
            get { return Mathf.Max(1, Mathf.CeilToInt((float)NumEvents / PageSize)); }
        }

        private int CurrentPage { get; set; } = DEFAULT_CURRENT_PAGE;
        private int PageSize { get; set; } = DEFAULT_PAGE_SIZE;

        [SerializeField] private InventoryRecord inventoryRecord;
        [SerializeField] private InventoryItemCatalogueAssetReference inventoryItemCatalogue;

        private const int DEFAULT_PAGE_SIZE = 10;
        private const int DEFAULT_CURRENT_PAGE = 0;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return inventoryItemCatalogue.ShouldLoad;
        }

        public IEnumerator LoadAssets()
        {
            yield return inventoryItemCatalogue.LoadAssetAsync();
        }

        #endregion

        protected override void OnDrawMenu()
        {
            int deferredRemove = -1;

            for (int i = 0, n = inventoryRecord.NumItems; i < n; ++i)
            {
                InventoryItem inventoryItem = inventoryRecord.GetItem(i);

                using (var horizontal = new HorizontalScope())
                {
                    Label(inventoryItem.DisplayName);

                    if (Button("Remove", ExpandWidth(false)))
                    {
                        deferredRemove = i;
                    }
                }
            }

            if (deferredRemove != -1)
            {
                inventoryRecord.RemoveItem(deferredRemove);
            }

            Space(4);
            Label("Item Catalogue");

            using (var horizontal = new HorizontalScope())
            {
                Label("Page Size");

                string pageSizeText = PageSize.ToString();
                pageSizeText = TextField(pageSizeText);

                if (Button("Set", ExpandWidth(false)) && int.TryParse(pageSizeText, out int newPageSize))
                {
                    PageSize = newPageSize > 0 ? newPageSize : PageSize;
                }
            }

            using (var horizontal = new HorizontalScope())
            {
                Label("Page");
                
                string currentPageText = CurrentPage.ToString();
                currentPageText = TextField(currentPageText);
                Label($" / {NumPages}", ExpandWidth(false));

                if (Button("Set", ExpandWidth(false)) && int.TryParse(currentPageText, out int newCurrentPage))
                {
                    // The text is one indexed for readibility, but is stored 0 indexed
                    CurrentPage = newCurrentPage - 1;
                }
            }

            using (var horizontal = new HorizontalScope())
            {
                if (CurrentPage > 0 && Button("<", ExpandWidth(false), Width(40)))
                {
                    --CurrentPage;
                }

                FlexibleSpace();

                if (CurrentPage < NumPages - 1 && Button(">", ExpandWidth(false), Width(40)))
                {
                    ++CurrentPage;
                }
            }

            int firstItemIndex = CurrentPage * PageSize;
            int numEventsToShow = Mathf.Min(PageSize, NumEvents - firstItemIndex);

            for (int itemIndex = firstItemIndex; itemIndex < firstItemIndex + numEventsToShow; ++itemIndex)
            {
                InventoryItem inventoryItem = inventoryItemCatalogue.Asset.GetItem(itemIndex);
                Label(inventoryItem.DisplayName);

                if (!inventoryRecord.IsFull && Button("Add", ExpandWidth(false)))
                {
                    inventoryRecord.AddItem(inventoryItem);
                }
            }
        }
    }
}