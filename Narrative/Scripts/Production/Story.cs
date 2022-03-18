using Celeste.DataStructures;
using System;
using System.Collections.Generic;
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
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public string StoryName
        {
            get { return storyName; }
            set
            {
                storyName = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public string StoryDescription
        {
            get { return storyDescription; }
            set
            {
                storyDescription = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Sprite StoryThumbnail
        {
            get { return storyThumbnail; }
        }

        public int NumChapters 
        {
            get { return chapters.Count; }
        }

        [SerializeField] private int guid;
        [SerializeField] private string storyName;
        [SerializeField] private string storyDescription;
        [SerializeField] private Sprite storyThumbnail;
        [SerializeField] private List<Chapter> chapters = new List<Chapter>();

        #endregion

        public Chapter GetChapter(int index)
        {
            return chapters.Get(index);
        }

        public Chapter FindChapter(int guid)
        {
            return chapters.Find(x => x.Guid == guid);
        }

        public void AddChapter(Chapter chapter)
        {
            chapters.Add(chapter);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}