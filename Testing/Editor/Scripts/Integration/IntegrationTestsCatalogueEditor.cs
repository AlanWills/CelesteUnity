using Celeste.Testing;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Testing
{
    [CustomEditor(typeof(IntegrationTestsCatalogue))]
    public class IntegrationTestsCatalogueEditor : IIndexableItemsEditor<IntegrationTest>
    {
    }
}
