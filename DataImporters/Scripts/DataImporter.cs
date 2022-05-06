using System.Collections;
using UnityEngine;

namespace Celeste.DataImporters
{
    public abstract class DataImporter : ScriptableObject
    {
        public abstract IEnumerator Import();
    }
}
