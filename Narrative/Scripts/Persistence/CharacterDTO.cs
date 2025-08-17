using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public struct CharacterDTO
    {
        public int guid;
        public string name;

        public CharacterDTO(CharacterRecord characterRecord)
        {
            guid = characterRecord.Character.Guid;
            name = characterRecord.Character.CharacterName;
        }
    }
}