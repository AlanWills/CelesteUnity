using Celeste.Objects;
using System.Collections;
using UnityEngine;

namespace Celeste.DataImporters
{
    [CreateAssetMenu(fileName = nameof(DataImporterCatalogue), menuName = "Celeste/Data Importers/Data Importer Catalogue")]
    public class DataImporterCatalogue : ArrayScriptableObject<DataImporter>
    {
        public IEnumerator ImportAll()
        {
            for (int i = 0, n = NumItems; i < n; i++)
            {
                yield return GetItem(i).Import();
            }

            Debug.Log($"{name}: Import All done!", this);
        }
    }
}
