using UnityEngine;

namespace Celeste.Objects
{
    public class CatalogueScriptableObject<T> : ListScriptableObject<T>, IAutomaticImportSettings
    {
        #region Properties and Fields

        public AutomaticImportBehaviour ImportBehaviour => importBehaviour;

        [SerializeField] private AutomaticImportBehaviour importBehaviour;

        #endregion
    }
}