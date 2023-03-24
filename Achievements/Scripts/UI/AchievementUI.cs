using UnityEngine;
using PolyAndCode.UI;
using Celeste.Achievements.Objects;
using Celeste.Localisation.UI;
using Celeste.Rewards.UI;
using TMPro;
using UnityEngine.UI;

namespace Celeste.Achievements.UI
{
	public class AchievementUI : MonoBehaviour, ICell
	{
		#region Properties and Fields
		
		[SerializeField] private string progressFormat = "{0} / {1}";

		[Header("UI Elements")]
		[SerializeField] private LocalisedTextUI achievementTitle;
		[SerializeField] private TextMeshProUGUI achievementProgressText;
		[SerializeField] private Slider achievementProgressSlider;
		[SerializeField] private GameObject inProgressUI;
		[SerializeField] private GameObject achievedUI;
		[SerializeField] private GameObject collectedUI;
		[SerializeField] private RewardUI rewardUI;

		private Achievement achievement;

		#endregion

		public void Hookup(AchievementUIData achievementUIData)
		{
			achievement = achievementUIData.Achievement;
			achievement.AddOnStateChangedCallback(OnAchievementStateChanged);

			RefreshUI();
		}

		private void RefreshUI()
		{
			AchievementState state = achievement.State;

			achievementTitle.Localise(achievement.Title);
			inProgressUI.SetActive(state == AchievementState.InProgress);
			achievedUI.SetActive(state == AchievementState.Achieved);
			collectedUI.SetActive(state == AchievementState.Collected);
			rewardUI.Hookup(achievement.Reward);

			bool achievementHasProgress = achievement.TryGetProgress(out var progress);
			bool showProgressUI = achievementHasProgress && state == AchievementState.InProgress;
			achievementProgressText.gameObject.SetActive(showProgressUI);
			achievementProgressSlider.gameObject.SetActive(showProgressUI);

			if (showProgressUI)
			{
				achievementProgressText.text = string.Format(progressFormat, progress.currentProgress, progress.requiredProgress);
				achievementProgressSlider.value = progress.currentProgress;
				achievementProgressSlider.maxValue = progress.requiredProgress;
			}
		}

		#region Unity Methods

		private void OnDisable()
		{
			if (achievement != null)
			{
				achievement.RemoveOnStateChangedCallback(OnAchievementStateChanged);
				achievement = null;
			}
		}

		#endregion
		
		#region Callbacks

		public void OnCollectPressed()
		{
			achievement.Collect();
		}

		private void OnAchievementStateChanged(Achievement achievement)
		{
			RefreshUI();
		}
		
		#endregion
	}
}
