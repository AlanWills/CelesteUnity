using Celeste.Narrative.Characters;
using Celeste.Narrative.Parameters;
using Celeste.Persistence;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Celeste.Narrative.Persistence
{
    [AddComponentMenu("Celeste/Narrative/Persistence/Narrative Persistence")]
    public class NarrativePersistence : PersistentSceneSingleton<NarrativePersistence, ProductionDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Narrative.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private StoryCatalogue storyCatalogue;
        [SerializeField] private ChapterRecordValue currentChapterRecord;

        [NonSerialized] private ProductionRecord productionRecord = new ProductionRecord();

        #endregion

        public StoryRecord FindOrAddStoryRecord(Story story)
        {
            return productionRecord.FindOrAddStoryRecord(story);
        }

        public ChapterRecord FindOrAddChapterRecord(Story story, Chapter chapter)
        {
            StoryRecord storyRecord = productionRecord.FindOrAddStoryRecord(story);
            return storyRecord.FindOrAddChapterRecord(chapter);
        }

        public ChapterRecord FindLastPlayedChapterRecord()
        {
            Story story = storyCatalogue.FindByGuid(productionRecord.LastPlayedStoryGuid);
            if (story == null)
            {
                return null;
            }

            Chapter chapter = story.FindChapter(productionRecord.LastPlayedChapterGuid);
            if (chapter == null)
            {
                return null;
            }

            return FindOrAddChapterRecord(story, chapter);
        }

        #region Save/Load Methods

        protected override void Deserialize(ProductionDTO dto)
        {
            productionRecord.LastPlayedStoryGuid = dto.lastPlayedStoryGuid;
            productionRecord.LastPlayedChapterGuid = dto.lastPlayedChapterGuid;

            foreach (StoryDTO storyDTO in dto.stories)
            {
                Story story = storyCatalogue.FindByGuid(storyDTO.guid);
                UnityEngine.Debug.Assert(story != null, $"Could not find story with guid {storyDTO.guid}.");
                StoryRecord storyRecord = productionRecord.AddStoryRecord(story);

                foreach (ChapterDTO chapterDTO in storyDTO.chapters)
                {
                    Chapter chapter = story.FindChapter(chapterDTO.guid);
                    UnityEngine.Debug.Assert(chapter != null, $"Could not find Chapter with guid {chapterDTO.guid} in story {story.name}.");
                    ChapterRecord chapterRecord = storyRecord.AddChapterRecord(chapter, chapterDTO.currentNodeGuid);

                    foreach (CharacterDTO characterDTO in chapterDTO.characters)
                    {
                        CharacterRecord characterRecord = chapterRecord.FindCharacterRecord(characterDTO.guid);
                        UnityEngine.Debug.Assert(characterRecord != null, $"Could not find Character Record with guid {characterDTO.guid} in Chapter {chapter.name}.");
                        characterRecord.CharacterName = characterDTO.name;
                    }

                    foreach (ValueDTO valueDTO in chapterDTO.parameters)
                    {
                        ValueRecord valueRecord = chapterRecord.FindValueRecord(valueDTO.name);
                        UnityEngine.Debug.Assert(valueRecord != null, $"Could not find Value Record with name {valueDTO.name} in Chapter {chapter.name}.");
                        valueRecord.Value = valueDTO.value;
                    }
                }
            }
        }

        protected override ProductionDTO Serialize()
        {
            return new ProductionDTO(productionRecord);
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion

        #region Callbacks

        public void OnNarrativeBegun(NarrativeRuntime narrativeRuntime)
        {
            productionRecord.LastPlayedStoryGuid = narrativeRuntime.Record.StoryRecord.Story.Guid;
            productionRecord.LastPlayedChapterGuid = narrativeRuntime.Record.Chapter.Guid;
        }

        #endregion
    }
}