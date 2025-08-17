using System;
using Celeste.Narrative.Characters.Components;
using CelesteEditor.Tools.Utils;

namespace CelesteEditor.Narrative
{
    public static class NarrativeEditorConstants
    {
        #region Properties and Fields

        public static readonly Type[] AllCharacterComponentTypes;
        public static readonly string[] AllCharacterComponentDisplayNames;

        #endregion

        static NarrativeEditorConstants()
        {
            TypeExtensions.LoadTypes<CharacterComponent>(ref AllCharacterComponentTypes, ref AllCharacterComponentDisplayNames);
        }
    }
}