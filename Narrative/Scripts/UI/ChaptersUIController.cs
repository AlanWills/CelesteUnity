using System;
using System.Collections;
using System.Collections.Generic;
using Celeste.Tools;
using PolyAndCode.UI;
using UnityEngine;

namespace Celeste.Narrative.UI
{
	public class ChaptersUIController : MonoBehaviour, IRecyclableScrollRectDataSource
	{
		#region Properties and Fields

		[SerializeField] private RecyclableScrollRect scrollRect;
		[SerializeField] private NarrativeRecord narrativeRecord;

		[NonSerialized] private List<ChapterUIData> chaptersCellData = new List<ChapterUIData>();

		#endregion

		#region Unity Methods

		private void OnValidate()
		{
			this.TryGetInChildren(ref scrollRect);
		}

		private IEnumerator Start()
		{
			// Spin wait whilst we load the story records (this can happen in the editor when loading straight into the scene)
			while (narrativeRecord.NumStoryRecords <= 0)
			{
				yield return null;
			}

			SetUpUI();
		}

		#endregion

		private void SetUpUI()
		{
			chaptersCellData.Clear();

			for (int storyIndex = 0, n = narrativeRecord.NumStoryRecords; storyIndex < n; ++storyIndex)
			{
				StoryRecord story = narrativeRecord.GetStoryRecord(storyIndex);

				for (int chapterIndex = 0; chapterIndex < story.NumChapterRecords; ++chapterIndex)
				{
					chaptersCellData.Add(new ChapterUIData(story.GetChapterRecord(chapterIndex)));
				}
			}

			scrollRect.Initialize(this);
		}

		#region IRecyclableScrollRectDataSource

		public int GetItemCount()
		{
			return chaptersCellData.Count;
		}

		public void SetCell(ICell cell, int index)
		{
			(cell as ChapterUI).Hookup(chaptersCellData[index]);
		}

		#endregion
	}
}
