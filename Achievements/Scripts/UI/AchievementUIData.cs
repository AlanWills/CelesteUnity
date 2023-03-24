using Celeste.Achievements.Objects;

namespace Celeste.Achievements.UI
{
	public class AchievementUIData
	{
		#region Properties and Fields

		public Achievement Achievement { get; }

		#endregion

		public AchievementUIData(Achievement achievement)
		{
			Achievement = achievement;
		}
	}
}
