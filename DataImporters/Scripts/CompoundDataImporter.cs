using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataImporters
{
    [CreateAssetMenu(fileName = nameof(CompoundDataImporter), menuName = "Celeste/Data Importers/Compound Data Importer")]
    public class CompoundDataImporter : DataImporter
    {
        #region Properties and Fields

        [SerializeField] private List<DataImporter> dataImporters = new List<DataImporter>();

        #endregion

        protected override IEnumerator DoImport()
        {
            for (int i = 0; i < dataImporters.Count; i++)
            {
                Debug.Assert(dataImporters[i] != null, $"Data importer {i} was null on compound importer {name}.");
                yield return dataImporters[i].Import();
            }
        }
    }
}
