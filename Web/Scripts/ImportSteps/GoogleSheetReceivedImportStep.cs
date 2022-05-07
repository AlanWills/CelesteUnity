using UnityEngine;

namespace Celeste.Web.ImportSteps
{
    public abstract class GoogleSheetReceivedImportStep : ScriptableObject
    {
        public abstract void Execute(GoogleSheet googleSheet);
    }
}
