using Celeste.Twine;
using CelesteEditor.DataStructures;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Twine.Database
{
    [CustomEditor(typeof(TwineStoryDatabase))]
    public class TwineStoryDatabaseEditor : IIndexableItemsEditor<TwineStory>
    {
    }
}