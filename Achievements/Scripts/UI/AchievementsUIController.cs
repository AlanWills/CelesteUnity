using System.Collections.Generic;
using Celeste.Achievements.Objects;
using Celeste.Tools;
using PolyAndCode.UI;
using UnityEngine;

namespace Celeste.Achievements.UI
{
	public class AchievementsUIController : MonoBehaviour, IRecyclableScrollRectDataSource
	{
		#region Properties and Fields

		[SerializeField] private RecyclableScrollRect scrollRect;
		[SerializeField] private AchievementRecord achievementRecord;

		private List<AchievementUIData> achievementCellData = new List<AchievementUIData>();

		#endregion

		#region Unity Methods

		private void OnValidate()
		{
			this.TryGetInChildren(ref scrollRect);
		}
		
		private void Start()
		{
			SetUpUI();
		}

		#endregion

		private void SetUpUI()
		{
			for (int i = 0, n = achievementRecord.NumAchievements; i < n; ++i)
			{
				Achievement achievement = achievementRecord.GetAchievement(i);
				achievementCellData.Add(new AchievementUIData(achievement));
			}

			scrollRect.Initialize(this);
		}

		#region IRecyclableScrollRectDataSource

		public int GetItemCount()
		{
			return achievementCellData.Count;
		}

		public void SetCell(ICell cell, int index)
		{
			(cell as AchievementUI).Hookup(achievementCellData[index]);
		}

		#endregion
	}
}
