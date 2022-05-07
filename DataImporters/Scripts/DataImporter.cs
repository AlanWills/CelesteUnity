using Celeste.DataImporters.ImportSteps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.DataImporters
{
    public abstract class DataImporter : ScriptableObject
    {
        #region Properties and Fields

        [SerializeField] private List<ImportStep> preImportSteps = new List<ImportStep>();
        [SerializeField] private List<ImportStep> postImportSteps = new List<ImportStep>();

        #endregion

        public IEnumerator Import()
        {
            for (int i = 0; i < preImportSteps.Count; i++)
            {
                Debug.Assert(preImportSteps[i] != null, $"Pre import step {i} was null on importer {name}.");
                preImportSteps[i].Execute();
            }

            yield return DoImport();

            for (int i = 0; i < postImportSteps.Count; i++)
            {
                Debug.Assert(postImportSteps[i] != null, $"Post import step {i} was null on importer {name}.");
                postImportSteps[i].Execute();
            }
        }

        protected abstract IEnumerator DoImport();
    }
}
