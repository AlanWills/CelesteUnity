using UnityEngine;

namespace Celeste.DataImporters.ImportSteps
{
    public abstract class ImportStep : ScriptableObject
    {
        public abstract void Execute();
    }
}
