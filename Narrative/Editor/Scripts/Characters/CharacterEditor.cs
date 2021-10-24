﻿using Celeste.Narrative.Characters;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.Characters
{
    [CustomEditor(typeof(Character))]
    public class CharacterEditor : Editor
    {
        #region Create Menu Item

        [MenuItem("Assets/Create/Celeste/Narrative/Characters/Character")]
        public static void CreateCharacterMenuItem()
        {
            Character character = Character.Create(nameof(Character), AssetUtility.GetSelectionObjectPath());
            AssetUtility.SelectAsset(character);
        }

        #endregion

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Repair"))
            {
                Character character = target as Character;
                character.Repair();
            }

            base.OnInspectorGUI();
        }
    }
}
