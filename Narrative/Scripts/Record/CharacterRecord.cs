using Celeste.Narrative.Characters;
using System;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    [Serializable]
    public class CharacterRecord
    {
        #region Properties and Fields

        public Character Character { get; }

        public string CharacterName
        {
            get => Character.CharacterName;
            set => Character.CharacterName = value;
        }

        #endregion

        public CharacterRecord(Character character)
        {
            Character = character;
        }
    }
}