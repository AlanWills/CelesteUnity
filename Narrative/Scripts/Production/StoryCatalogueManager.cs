﻿using Celeste.Assets;
using Celeste.Twine;
using System.Collections;
using UnityEngine;

namespace Celeste.Narrative.Assets
{
    [AddComponentMenu("Celeste/Narrative/Story Catalogue Manager")]
    public class StoryCatalogueManager : MonoBehaviour, IHasAssets
    {
        #region Properties and Fields

        [SerializeField] private StoryCatalogueAssetReference storyCatalogue;
        [SerializeField] private TwineRecord twineRecord;

        private const int STORY_GUID_FOR_TWINE_RECORD = 9999999;

        #endregion

        #region IHasAssets

        public bool ShouldLoadAssets()
        {
            return true;
        }

        public IEnumerator LoadAssets()
        {
            yield return storyCatalogue.LoadAssetAsync<StoryCatalogue>();

            AddMissingTwineRecordStories();
        }

        #endregion

        #region Callbacks

        public void OnAddMissingTwineRecordStories()
        {
            AddMissingTwineRecordStories();
        }

        #endregion

        private void AddMissingTwineRecordStories()
        {
            // Add twine record stories
            if (twineRecord.NumTwineStoryRecords > 0)
            {
                Story twineRecordStory = storyCatalogue.Asset.FindByGuid(STORY_GUID_FOR_TWINE_RECORD);

                if (twineRecordStory == null)
                {
                    twineRecordStory = ScriptableObject.CreateInstance<Story>();
                    twineRecordStory.name = "TwineRecordStories";
                    twineRecordStory.StoryName = "Twine Record Stories";
                    twineRecordStory.Guid = STORY_GUID_FOR_TWINE_RECORD;
                    twineRecordStory.StoryDescription = "Stories automatically generated from the twine record.";

                    storyCatalogue.Asset.AddItem(twineRecordStory);
                }

                for (int i = 0, n = twineRecord.NumTwineStoryRecords; i < n; ++i)
                {
                    TwineStory twineStory = twineRecord.LoadTwineStory(i);

                    // Need to add this chapter because it's missing from our story
                    if (twineRecordStory.FindChapter(i + 1) == null)
                    {
                        Chapter chapter = ScriptableObject.CreateInstance<Chapter>();
                        chapter.name = twineStory.name;
                        chapter.ChapterName = twineStory.name;
                        chapter.ChapterDescription = "Chapter automatically generated from twine record story.";
                        chapter.Guid = i + 1;
                        chapter.TwineStory = twineStory;
                        twineRecordStory.AddChapter(chapter);
                    }
                }
            }
        }
    }
}
