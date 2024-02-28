using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataImporters
{
    [CreateAssetMenu(
        fileName = nameof(CompoundDataImporter), 
        menuName = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM + "Compound Data Importer",
        order = CelesteMenuItemConstants.DATAIMPORTERS_MENU_ITEM_PRIORITY)]
    public class CompoundDataImporter : DataImporter
    {
        #region Properties and Fields

        [SerializeField] private List<DataImporter> dataImporters = new List<DataImporter>();

        #endregion

        protected override IEnumerator DoImport(Action<string, float> progressCallback = null)
        {
            for (int i = 0, n = dataImporters.Count; i < n; ++i)
            {
                DataImporter importer = dataImporters[i];
                Debug.Assert(importer != null, $"Data importer {i} was null on compound importer {name}.");
                progressCallback?.Invoke($"Importing {importer.name}", i / (float)n);
                
                yield return importer.Import((s, f) =>
                {
                    progressCallback?.Invoke($"{importer.name}: {s}", (i + f) / n);
                });
            }
        }
    }
}
