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
        public int AvatarCustomisationGuid { get; set; }

        public string CharacterName
        {
            get { return Character.CharacterName; }
            set { Character.CharacterName = value; }
        }

        public Sprite CharacterAvatarIcon
        {
            get { return Character.CharacterAvatarIcon; }
            set { Character.CharacterAvatarIcon = value; }
        }

        #endregion

        public CharacterRecord(Character character)
        {
            Character = character;
        }
    }
}