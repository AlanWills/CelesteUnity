using System;
using UnityEngine;

namespace Celeste.Objects
{
    [Serializable]
    public struct AutomaticImportAssetSettings
    {
        public static AutomaticImportAssetSettings Add => new AutomaticImportAssetSettings
        {
            addToCatalogue = true
        };
        
        public static AutomaticImportAssetSettings DontAdd => new AutomaticImportAssetSettings
        {
            addToCatalogue = false
        };
        
        public bool AddToCatalogue => addToCatalogue;
        
        [SerializeField] private bool addToCatalogue;
    }
    
    public interface IAutomaticImportAssetSettings
    {
        AutomaticImportAssetSettings ImportSettings { get; }
    }
}