using System;
using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.DataImporters
{
    [CreateAssetMenu(
        fileName = nameof(DataImporterCatalogue), 
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Data Importer Catalogue",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
    public class DataImporterCatalogue : ArrayScriptableObject<DataImporter>
    {
        public IEnumerator ImportAll(
            Action<string, float> progressCallback = null,
            Action completeCallback = null)
        {
            for (int i = 0, n = NumItems; i < n; i++)
            {
                DataImporter importer = GetItem(i);
                progressCallback?.Invoke($"Importing {importer.name}", i / (float)n);
                
                yield return importer.Import((s, f) =>
                {
                    progressCallback?.Invoke($"Importing {importer.name}.  {s}", (i + f) / n);
                });
            }

            progressCallback?.Invoke($"Finished importing {NumItems} importers!", 1);
            completeCallback?.Invoke();
            Debug.Log($"{name}: Import All done!", this);
        }
    }
}
