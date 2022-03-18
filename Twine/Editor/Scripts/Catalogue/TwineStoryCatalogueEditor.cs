using Celeste.Twine;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Twine.Catalogue
{
    [CustomEditor(typeof(TwineStoryCatalogue))]
    public class TwineStoryCatalogueEditor : IIndexableItemsEditor<TwineStory>
    {
    }
}