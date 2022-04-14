using Celeste.Assets;
using Celeste.Narrative.Assets;
using Celeste.Narrative.Persistence;
using Celeste.Persistence;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative
{
    [AddComponentMenu("Celeste/Narrative/Narrative Manager")]
    public class NarrativeManager : PersistentSceneManager<NarrativeManager, ProductionDTO>, IHasAssets
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Narrative.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private StoryCatalogueAssetReference storyCatalogue;
        [SerializeField] private NarrativeRecord narrativeRecord;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            yield return storyCatalogue.LoadAssetAsync<StoryCatalogue>();

            Load();
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(ProductionDTO dto)
        {
            narrativeRecord.LastPlayedStoryGuid = dto.lastPlayedStoryGuid;
            narrativeRecord.LastPlayedChapterGuid = dto.lastPlayedChapterGuid;

            foreach (StoryDTO storyDTO in dto.stories)
            {
                Story story = storyCatalogue.Asset.FindByGuid(storyDTO.guid);
                UnityEngine.Debug.Assert(story != null, $"Could not find story with guid {storyDTO.guid}.");
                StoryRecord storyRecord = narrativeRecord.AddStoryRecord(story);

                foreach (ChapterDTO chapterDTO in storyDTO.chapters)
                {
                    Chapter chapter = story.FindChapter(chapterDTO.guid);
                    UnityEngine.Debug.Assert(chapter != null, $"Could not find Chapter with guid {chapterDTO.guid} in story {story.name}.");
                    ChapterRecord chapterRecord = storyRecord.AddChapterRecord(chapter, chapterDTO.currentNodePath);

                    chapterRecord.CurrentBackgroundGuid = chapterDTO.currentBackgroundGuid;

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
            return new ProductionDTO(narrativeRecord);
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion

        #region Callbacks

        public void OnNarrativeBegun(NarrativeRuntime narrativeRuntime)
        {
            narrativeRecord.LastPlayedStoryGuid = narrativeRuntime.Record.StoryRecord.Story.Guid;
            narrativeRecord.LastPlayedChapterGuid = narrativeRuntime.Record.Chapter.Guid;
        }

        #endregion
    }
}