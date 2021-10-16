using System;
using System.Collections;
using UnityEngine;

namespace Celeste.Objects
{
    public class AssetDatabasePathAttribute : Attribute
    {
        public string AssetDatabasePath { get; }

        public AssetDatabasePathAttribute(string assetDatabasePath)
        {
            AssetDatabasePath = assetDatabasePath;
        }
    }
}