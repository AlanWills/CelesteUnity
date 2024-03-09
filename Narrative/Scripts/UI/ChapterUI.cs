using UnityEngine;
using PolyAndCode.UI;
using TMPro;
using Celeste.Narrative.Parameters;

namespace Celeste.Narrative.UI
{
	public class ChapterUI : MonoBehaviour, ICell
	{
		#region Properties and Fields

		[Header("UI Elements")]
		[SerializeField] private TextMeshProUGUI titleText;

		[Header("Data")]
		[SerializeField] private ChapterRecordValue chosenChapter;

		private ChapterRecord chapter;

		#endregion

		public void Hookup(ChapterUIData chapterUIData)
		{
			chapter = chapterUIData.Chapter;
			titleText.text = chapter.ChapterName;
		}

		#region Unity Methods

		private void OnDisable()
		{
			chapter = null;
		}

		#endregion

		#region Callbacks

		public void SelectChapter()
		{
			chosenChapter.Value = chapter;
		}

		#endregion
	}
}
