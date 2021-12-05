using Celeste.Wallet;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Wallet
{
    [CustomEditor(typeof(CurrencyCatalogue))]
    public class CurrencyCatalogueEditor : IIndexableItemsEditor<Currency>
    {
    }
}
