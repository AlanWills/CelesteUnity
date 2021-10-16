using Celeste.DataStructures;
using System;
using UnityEditor;
using UnityEngine;

namespace Celeste.Narrative
{
    [CreateAssetMenu(fileName = "Story", menuName = "Celeste/Narrative/Production/Story")]
    public class Story : ScriptableObject
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
        }

        public string StoryName
        {
            get { return storyName; }
        }

        public string StoryDescription
        {
            get { return storyDescription; }
        }

        public Sprite StoryThumbnail
        {
            get { return storyThumbnail; }
        }

        public int NumChapters 
        {
            get 
            {
#if NULL_CHECKS
                if (chapters == null)
                {
                    return 0;
                }
#endif
                return chapters.Length; 
            }
        }

        [SerializeField] private int guid;
        [SerializeField] private string storyName;
        [SerializeField] private string storyDescription;
        [SerializeField] private Sprite storyThumbnail;
        [SerializeField] private Chapter[] chapters;

        #endregion

        public Chapter GetChapter(int index)
        {
            return chapters.Get(index);
        }

        public Chapter FindChapter(int guid)
        {
            return Array.Find(chapters, x => x.Guid == guid);
        }
    }
}