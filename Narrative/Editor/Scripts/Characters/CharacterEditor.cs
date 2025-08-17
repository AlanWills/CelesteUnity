using System;
using Celeste.Narrative.Characters;
using Celeste.Narrative.Characters.Components;
using CelesteEditor.Components;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Characters
{
    [CustomEditor(typeof(Character))]
    public class CharacterEditor : ComponentContainerUsingSubAssetsEditor<CharacterComponent>
    {
        protected override Type[] AllComponentTypes => NarrativeEditorConstants.AllCharacterComponentTypes;
        protected override string[] AllComponentDisplayNames => NarrativeEditorConstants.AllCharacterComponentDisplayNames;

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Initialize"))
            {
                Character character = target as Character;
                character.Editor_Initialize();
            }

            base.OnInspectorGUI();
        }
    }
}
