using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [Serializable]
    public struct ChapterDTO
    {
        public int guid;
        public string currentNodeGuid;
        public int currentBackgroundGuid;
        public List<CharacterDTO> characters;
        public List<ValueDTO> parameters;

        public ChapterDTO(ChapterRecord chapterRecord)
        {
            guid = chapterRecord.Chapter.Guid;
            currentNodeGuid = chapterRecord.CurrentNodeGuid;
            currentBackgroundGuid = chapterRecord.CurrentBackgroundGuid;

            characters = new List<CharacterDTO>(chapterRecord.NumCharacterRecords);
            for (int i = 0, n = chapterRecord.NumCharacterRecords; i < n; ++i)
            {
                characters.Add(new CharacterDTO(chapterRecord.GetCharacterRecord(i)));
            }

            parameters = new List<ValueDTO>(chapterRecord.NumValueRecords);
            for (int i = 0, n = chapterRecord.NumValueRecords; i < n; ++i)
            {
                parameters.Add(new ValueDTO(chapterRecord.GetValueRecord(i)));
            }
        }
    }
}